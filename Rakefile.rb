#!/usr/bin/env ruby

require 'albacore'
require 'fileutils'

CONFIG        = 'Debug'
RAKE_DIR      = File.expand_path(File.dirname(__FILE__))
SOLUTION_DIR  = RAKE_DIR + "/Highway"
TEMPLATE_DIR  = RAKE_DIR + "/Templates"
TEST_DIR      = SOLUTION_DIR + "/test/"
SRC_DIR       = SOLUTION_DIR + "/src/"
SOLUTION_FILE = 'Highway.sln'
TEMPLATE_FILE = 'Templates.MVC.sln'
MSTEST        = ENV['VS110COMNTOOLS'] + "..\\IDE\\mstest.exe"
NUGET         = SOLUTION_DIR + "/.nuget/nuget.exe"

# --- Retrieve a list of all Test DLLS -------------------------------------------------------
Dir.chdir('Highway/test')
TEST_DLLS     = Dir.glob('*Tests').collect{|dll| File.join(dll, 'bin', CONFIG, dll + '.dll')}.map{|dll| 'Highway/test/' + dll }
Dir.chdir('../..')
# --------------------------------------------------------------------------------------------

task :default                     => ['build:msbuild', 'build:templates']
task :test                        => ['build:mstest' ]
task :package                     => ['package:packall']
task :push                        => ['package:pushall']

namespace :build do

  msbuild :msbuild, [:targets] do |msb, args|
    args.with_defaults(:targets => :Build)
    msb.properties :configuration => CONFIG
    msb.targets args[:targets]
    msb.solution = "#{SOLUTION_DIR}/#{SOLUTION_FILE}"
  end
  
  desc "MSTest Test Runner Example"
	mstest :mstest => :msbuild do |mstest|
	    mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
	    mstest.assemblies TEST_DLLS
	end
	
	msbuild :template_build, [:targets] do |msb, args|
    args.with_defaults(:targets => :Build)
    msb.properties :configuration => CONFIG
    msb.targets args[:targets]
    msb.solution = "#{TEMPLATE_DIR}/#{TEMPLATE_FILE}"
  end
	
	task :templates => [ :template_build, :clean_templates_build, :create_templates_build ] do
		appstart_files = Dir.glob('Templates/Templates.MVC/App_Start/*.cs')
		basetypes_files = Dir.glob('Templates/Templates.MVC/BaseTypes/*.cs')
		models_files = Dir.glob('Templates/Templates.MVC/Models/*.cs')
		installers_files = Dir.glob('Templates/Templates.MVC/Installers/*.cs')
		filters_files = Dir.glob('Templates/Templates.MVC/Filters/*.cs')
		
		appstart_files.each do |file|
			out_filename = 'Templates/build/content/App_Start/' +  File.basename(file) + '.pp'
			File.open(out_filename,'w+') do |output_file|
				output_file.puts File.read(file).gsub(/Templates\./,'$rootnamespace$.')
			end
		end
		
		basetypes_files.each do |file|
			out_filename = 'Templates/build/content/BaseTypes/' +  File.basename(file) + '.pp'
			File.open(out_filename,'w+') do |output_file|
				output_file.puts File.read(file).gsub(/Templates\./,'$rootnamespace$.')
			end
		end
		
		models_files.each do |file|
			out_filename = 'Templates/build/content/Models/' +  File.basename(file) + '.pp'
			File.open(out_filename,'w+') do |output_file|
				output_file.puts File.read(file).gsub(/Templates\./,'$rootnamespace$.')
			end
		end
		
		installers_files.each do |file|
			out_filename = 'Templates/build/content/Installers/' +  File.basename(file) + '.pp'
			File.open(out_filename,'w+') do |output_file|
				output_file.puts File.read(file).gsub(/Templates\./,'$rootnamespace$.')
			end
		end
		
		filters_files.each do |file|
			out_filename = 'Templates/build/content/Filters/' +  File.basename(file) + '.pp'
			File.open(out_filename,'w+') do |output_file|
				output_file.puts File.read(file).gsub(/Templates\./,'$rootnamespace$.')
			end
		end
		
		cp 'Templates/Templates.Mvc/log4net.config', 'Templates/build/content/'
		cp 'Templates/Templates.Mvc/Highway.Mvc.Castle.nuspec', 'Templates/build/'
	end
	
	task :clean_templates_build do
		sh 'rm -rf Templates/build/'
	end
	
	task :create_templates_build do
		Dir.mkdir('Templates/build')
		Dir.mkdir('Templates/build/lib')
		Dir.mkdir('Templates/build/tools')
		Dir.mkdir('Templates/build/content')
		Dir.mkdir('Templates/build/content/App_Start')
		Dir.mkdir('Templates/build/content/BaseTypes')
		Dir.mkdir('Templates/build/content/Models')
		Dir.mkdir('Templates/build/content/Installers')
		Dir.mkdir('Templates/build/content/Filters')
	end
end

namespace :package do
	
	def create_packs()
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data/Highway.Data.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework/Highway.Data.EntityFramework.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework.Castle/Highway.Data.EntityFramework.Castle.csproj -o pack'		
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework.Ninject/Highway.Data.EntityFramework.Ninject.csproj -o pack'	
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework.StructureMap/Highway.Data.EntityFramework.StructureMap.csproj -o pack'	
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework.Unity/Highway.Data.EntityFramework.Unity.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Test.MSTest/Highway.Test.MSTest.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Templates/build/Highway.Mvc.Castle.nuspec -o pack'
	end
		
	task :packall => [ :clean ] do
		Dir.mkdir('pack')
		create_lib
		create_packs	
		Dir.glob('pack/*') { |file| FileUtils.move(file,'nuget/') }
		clean_lib
		Dir.rmdir('pack')
	end
	
	task :pushall => [ :clean ] do
		Dir.mkdir('pack')
		create_lib
		create_packs	
		Dir.chdir('pack')
		Dir.glob('*').each do |file| 
			sh '../Highway/.nuget/nuget.exe push ' + file
			FileUtils.move(file,'../nuget/')
		end
		Dir.chdir('..')
		clean_lib
		Dir.rmdir('pack')
	end
	
	def clean_lib()
		FileUtils.remove_dir 'Highway/src/Highway.Data/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Data.EntityFramework/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Data.EntityFramework.Castle/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Data.EntityFramework.Ninject/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Data.EntityFramework.StructureMap/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Data.EntityFramework.Unity/lib', force = true
		FileUtils.remove_dir 'Highway/src/Highway.Test.MSTest/lib', force = true
	end
		
	def create_lib()
		Dir.mkdir('Highway/src/Highway.Data/lib')
		Dir.mkdir('Highway/src/Highway.Data/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data/bin/Debug/v4.0/.', 'Highway/src/Highway.Data/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data/bin/Debug/v4.5/.', 'Highway/src/Highway.Data/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework/lib')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework/bin/Debug/v4.0/.', 'Highway/src/Highway.Data.EntityFramework/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework/bin/Debug/v4.5/.', 'Highway/src/Highway.Data.EntityFramework/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Castle/lib')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Castle/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Castle/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Castle/bin/Debug/v4.0/.', 'Highway/src/Highway.Data.EntityFramework.Castle/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Castle/bin/Debug/v4.5/.', 'Highway/src/Highway.Data.EntityFramework.Castle/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Ninject/lib')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Ninject/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Ninject/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Ninject/bin/Debug/v4.0/.', 'Highway/src/Highway.Data.EntityFramework.Ninject/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Ninject/bin/Debug/v4.5/.', 'Highway/src/Highway.Data.EntityFramework.Ninject/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.StructureMap/lib')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.StructureMap/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.StructureMap/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.StructureMap/bin/Debug/v4.0/.', 'Highway/src/Highway.Data.EntityFramework.StructureMap/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.StructureMap/bin/Debug/v4.5/.', 'Highway/src/Highway.Data.EntityFramework.StructureMap/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Unity/lib')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Unity/lib/net40')
		Dir.mkdir('Highway/src/Highway.Data.EntityFramework.Unity/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Unity/bin/Debug/v4.0/.', 'Highway/src/Highway.Data.EntityFramework.Unity/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Data.EntityFramework.Unity/bin/Debug/v4.5/.', 'Highway/src/Highway.Data.EntityFramework.Unity/lib/net45/'
		Dir.mkdir('Highway/src/Highway.Test.MSTest/lib')
		Dir.mkdir('Highway/src/Highway.Test.MSTest/lib/net40')
		Dir.mkdir('Highway/src/Highway.Test.MSTest/lib/net45')
		FileUtils.cp_r 'Highway/src/Highway.Test.MSTest/bin/Debug/v4.0/.', 'Highway/src/Highway.Test.MSTest/lib/net40/'
		FileUtils.cp_r 'Highway/src/Highway.Test.MSTest/bin/Debug/v4.5/.', 'Highway/src/Highway.Test.MSTest/lib/net45/'		
	end
	
	task :clean do
		if Dir.exists? 'pack' 
			FileUtils.remove_dir 'pack', force = true
		end
	end
end

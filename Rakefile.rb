#!/usr/bin/env ruby

require 'albacore'
require 'fileutils'

CONFIG        = 'Debug'
RAKE_DIR      = File.expand_path(File.dirname(__FILE__))
SOLUTION_DIR  = RAKE_DIR + "/Highway"
TEST_DIR      = SOLUTION_DIR + "/test/"
SRC_DIR       = SOLUTION_DIR + "/src/"
SOLUTION_FILE = 'Highway.sln'
MSTEST        = ENV['VS110COMNTOOLS'] + "..\\IDE\\mstest.exe"
NUGET         = SOLUTION_DIR + "/.nuget/nuget.exe"

# --- Retrieve a list of all Test DLLS -------------------------------------------------------
Dir.chdir('Highway/test')
TEST_DLLS     = Dir.glob('*Tests').collect{|dll| File.join(dll, 'bin', CONFIG, dll + '.dll')}.map{|dll| 'Highway/test/' + dll }
Dir.chdir('../..')
# --------------------------------------------------------------------------------------------

task :default                     => ['build:msbuild']
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
end
	
namespace :package do
	
	def create_packs()
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data/Highway.Data.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.EntityFramework/Highway.Data.EntityFramework.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.NHibernate/Highway.Data.NHibernate.csproj -o pack'
		#sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Data.RavenDB/Highway.Data.RavenDB.csproj -o pack'
		sh 'Highway/.nuget/nuget.exe pack Highway/src/Highway.Test.MSTest/Highway.Test.MSTest.csproj -o pack'
	end
		
	task :packall => [ :clean ] do
		Dir.mkdir('pack')
		create_packs	
		Dir.glob('pack/*') { |file| FileUtils.move(file,'nuget/') }
		Dir.rmdir('pack')
	end
	
	task :pushall => [ :clean ] do
		Dir.mkdir('pack')
		create_packs	
		Dir.chdir('pack')
		Dir.glob('*').each do |file| 
			sh '../Highway/.nuget/nuget.exe push ' + file
			FileUtils.move(file,'../nuget/')
		end
		Dir.chdir('..')
		Dir.rmdir('pack')
	end
	
	task :clean do
		if Dir.exists? 'pack' 
			FileUtils.remove_dir 'pack', force = true
		end
	end
end

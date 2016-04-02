#------------------------------------------------------------------------------
# Full .NET Runtime Build & Test
#------------------------------------------------------------------------------

dnvm use 1.0.0-rc1-update2 -r clr
dnu restore --quiet
gci -recurse project.json | % {
  Write-Host -------------------------------------------------------------------------
  Write-Host "CLR Build $_"
  dnu build "$_" --framework dnx451 --quiet
}
pushd .\test\Highway.Data.DotNet.Tests
dnx test
popd
pushd .\test\Highway.Data.CoreCLR.Tests
dnx test
popd

#------------------------------------------------------------------------------
# CoreCLR Build & Test
#------------------------------------------------------------------------------
#
#dnvm use 1.0.0-rc1-update2 -r coreclr
#dnu restore --quiet
#$exclude = get-content exclude-coreclr.txt
#$bashlikepaths = gci -recurse project.json | % { $_.FullName.Replace("$PWD",".").Replace("\","/") }
#$bashlikepaths | ? { $exclude -notcontains $_ } | % {
#  Write-Host -------------------------------------------------------------------------
#  Write-Host "CoreCLR Build $_"
#  dnu build "$_" --framework dnxcore50 --quiet
#}
#pushd .\test\Highway.Data.CoreCLR.Tests
#dnx test
#popd

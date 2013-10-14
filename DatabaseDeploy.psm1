Push-Location; Import-Module SQLPS -DisableNameChecking; Pop-Location;
Import-Module PSWorkflow -DisableNameChecking

function Publish-DACPAC {
 [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$DACPAC = $null,  
 [Parameter(Position=1,Mandatory=1)] [string]$PublishProfile = $null,
 [Parameter(Position=2,Mandatory=1)] [string]$ConnectionString = $null,
 [Parameter(Position=3,Mandatory=1)] [string]$Database = $null,
 [Parameter(Position=4,Mandatory=0)] [switch]$Is2008R2 = $false
)  
    If($Is2008R2) {
        add-type -path "${Env:ProgramFiles(x86)}\Microsoft SQL Server\100\DAC\bin\Microsoft.SqlServer.Dac.dll"
    }
    else {
        add-type -path "${Env:ProgramFiles(x86)}\Microsoft SQL Server\110\DAC\bin\Microsoft.SqlServer.Dac.dll"
    }

    
    $dacProfile = [Microsoft.SqlServer.Dac.DacProfile]::Load($PublishProfile)
    Write-Verbose "Profile Loaded"
    $dacService = New-Object Microsoft.SqlServer.dac.dacservices ($ConnectionString)
    $dacPackage = [Microsoft.SqlServer.Dac.DacPackage]::Load($dacpac)
    Try { 
        register-objectevent -in $dacService -eventname Message -source "msg" -action { Out-host -in $Event.SourceArgs[1].Message.Message } | Out-Null 
    } 
    Catch {
        Write-Verbose "Could not attach listener"
    }
    Write-Verbose "Deploying the DB with the following settings" 
    Write-Verbose "SqlServer: $ConnectionString" 
    Write-Verbose "dacpac: $DACPAC" 
    Write-Verbose "Database: $Database"

    $dacService.deploy($dacPackage, $Database, $true, $dacProfile.DeployOptions)
    Try { 
        unregister-event -source "msg" 
    } 
    Catch {
        Write-Verbose "Unable to Detach listener"
    }
    Write-Output "Database Deployed"
}

workflow Publish-DACPACMultipleDatabases {
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$DACPAC,  
 [Parameter(Position=1,Mandatory=1)] [string]$PublishProfile,
 [Parameter(Position=2,Mandatory=1)] [string]$ConnectionString,
 [Parameter(Position=3,Mandatory=1)] [string[]]$Databases
)  
    foreach -parallel($Database in $Databases){
        Publish-DACPAC -DACPAC $DACPAC -PublishProfile $PublishProfile -ConnectionString $connectionString -Database $Database
    }
}

workflow Publish-DACPACMultipleServers {
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$DACPAC,
 [Parameter(Position=1,Mandatory=1)] [string]$PublishProfile,
 [Parameter(Position=2,Mandatory=1)] [string[]]$ConnectionStrings,
 [Parameter(Position=3,Mandatory=1)] [string]$Database
)  
    foreach -parallel($connectionString in $ConnectionStrings){
        Publish-DACPAC -DACPAC $DACPAC -PublishProfile $PublishProfile -ConnectionString $connectionString -Database $Database
    }
}

workflow Publish-DACPACMultipleServersMultipleDatabases {
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$DACPAC = "",  
 [Parameter(Position=1,Mandatory=1)] [string]$PublishProfile = "",
 [Parameter(Position=2,Mandatory=1)] [string[]]$ConnectionStrings,
 [Parameter(Position=3,Mandatory=1)] [string[]]$Databases
)  
    foreach -parallel($connectionString in $ConnectionStrings){
        Publish-DACPACMultipleDatabases -DACPAC $DACPAC -PublishProfile $PublishProfile -ConnectionString $connectionString -Databases $Databases
    }
}

workflow Invoke-SqlMultipleServersMultipleDatabases{
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$Query,
 [Parameter(Position=1,Mandatory=1)] [string[]]$ServerInstances,
 [Parameter(Position=2,Mandatory=1)] [string[]]$Databases
)  
    foreach -parallel($ServerInstance in $ServerInstances){
        Invoke-SqlMultipleDatabases -Query $Query -ServerInstance $ServerInstance -Databases $Databases
    }
}

workflow Invoke-SqlMultipleServers{
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$Query,
 [Parameter(Position=1,Mandatory=1)] [string[]]$ServerInstances,
 [Parameter(Position=2,Mandatory=1)] [string]$Database
)  
    foreach -parallel($ServerInstance in $ServerInstances){
        Invoke-SqlCmd -Query $Query -ServerInstance $ServerInstance -Database $Database
    }
}

workflow Invoke-SqlMultipleDatabases{
     [CmdletBinding()]  param(
 [Parameter(Position=0,Mandatory=1)] [string]$Query,
 [Parameter(Position=1,Mandatory=1)] [string]$ServerInstance,
 [Parameter(Position=2,Mandatory=1)] [string[]]$Databases
)  
    foreach -parallel($Database in $Databases){
        Invoke-SqlCmd -Query $Query -ServerInstance $ServerInstance -Database $Database
    }
}

Function Select-DatabaseExists {[CmdletBinding()]  
param(
 [Parameter(Position=0,Mandatory=1)] [string]$SqlServer = $null,  
 [Parameter(Position=1,Mandatory=1)] [string]$Database = $null
)  
 $exists = $FALSE
 try
 {
  $conn = New-Object system.Data.SqlClient.SqlConnection
  $conn.connectionstring = [string]::format("Server={0};Database={1};Integrated Security=SSPI;",$SqlServer,$Database)
  $conn.open()
  $exists = $true
 }
 catch
 {
  Write-Error "Failed to connect to DB $Database on $SqlServer"
 }
 
 Write-Output $exists
}

Function Select-TableExists {[CmdletBinding()]  
param(
 [Parameter(Position=0,Mandatory=1)] [string]$SqlServer = $null,  
 [Parameter(Position=1,Mandatory=1)] [string]$Database = $null,
 [Parameter(Position=2,Mandatory=1)] [string]$TableName = $null,
 [Parameter(Position=3,Mandatory=0)] [string]$SchemaName = "dbo"
)  
 $exists = $FALSE
 try
 {
    $query = [string]::Format("SELECT Count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'", $SchemaName, $TableName)
    $result = Invoke-Sqlcmd -Query $query -ServerInstance $SqlServer -Database $Database 
    if($result[0] -gt 0){
        $exists = $true
    }
    else{
        $exists = $FALSE
    }
 }
 catch
 {
  Write-Error "Failed to connect to DB $Database on $SqlServer"
 }
 
 Write-Output $exists
}
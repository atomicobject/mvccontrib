$dir = resolve-path .\

function DownloadFiles {
    remove-item "MVCContrib.Extras.release.zip" -ErrorAction:SilentlyContinue
    remove-item "MVCContrib.release.zip"  -ErrorAction:SilentlyContinue
    remove-item "MVCContrib.source.zip"  -ErrorAction:SilentlyContinue
    remove-item "MVCContrib.dll"  -ErrorAction:SilentlyContinue
    
    $extrasUrl  = "http://build-oss.headspringlabs.com/guestAuth/repository/download/bt10/.lastPinned/MVCContrib.Extras.release.zip"
    $releaseUrl = "http://build-oss.headspringlabs.com/guestAuth/repository/download/bt10/.lastPinned/MVCContrib.release.zip"
    $sourceUrl  = "http://build-oss.headspringlabs.com/guestAuth/repository/download/bt10/.lastPinned/MVCContrib.source.zip"
    $verDll =  "http://build-oss.headspringlabs.com/guestAuth/repository/download/bt10/.lastPinned/MvcContrib.dll"
    $clnt = new-object System.Net.WebClient

    $clnt.DownloadFile($extrasUrl,"$($dir)\MVCContrib.Extras.release.zip")
    $clnt.DownloadFile($releaseUrl, "$($dir)\MVCContrib.release.zip")
    $clnt.DownloadFile($sourceUrl, "$($dir)\MVCContrib.source.zip")
    $clnt.DownloadFile($verDll, "$($dir)\MVCContrib.dll")
    
}

DownloadFiles 

$ver = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$($dir)\MvcContrib.dll").FileVersion

& "..\bin\codeplex\createrelease.exe" $ver

.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -output:".\coverage.xml" -target:".\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe" -targetargs:"NHibernate.Json.Tests.dll" -filter:"+[NHibernate.*]* -[*.Tests]*" -targetdir:NHibernate.Json.Tests\bin -register:user

.\packages\coveralls.net.0.7.0\tools\csmacnz.Coveralls.exe --opencover -i .\coverage.xml 




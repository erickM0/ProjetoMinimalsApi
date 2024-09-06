using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Infra
{
    [TestClass]
    public class TestDatabase
    {
        [TestInitialize]
        public void Setup(){
            RestoreDatabase();
        }

        private void RestoreDatabase(){

            var dumpFile = Directory.GetCurrentDirectory()+"/../../../TestDb.dump.sql";
            var containerName = "MySql";

            var copyProcessInfo = new ProcessStartInfo {
                FileName = "docker",
                Arguments = $"cp {dumpFile} {containerName}:/tmp/dump.sql",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow =true,
            };

            var copyProcess = new Process{
                StartInfo = copyProcessInfo
            };

            copyProcess.Start();
            string copyError = copyProcess.StandardError.ReadToEnd();
            copyProcess.WaitForExit();

            if(copyProcess.ExitCode != 0){
                throw new Exception($"Error on database restore {copyError}");
            }       

            var restoreProcessInfo = new ProcessStartInfo {
                FileName = "docker",
                Arguments = $"exec -i {containerName} bash -c \"mysql -uroot -p'Senha123' CarrosDB < /tmp/TestDb.sql\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow =true,
            };

            var process = new Process{
                StartInfo = restoreProcessInfo
            };

            process.Start();
            string outpup = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if(process.ExitCode != 0){
                throw new Exception($"Error on database restore {error}");
            }

        }
    }
}
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Force.Crc32;

class Program
{
    static void Main(string[] args)
    {
        var votingSystem = new VotingSystem();
        votingSystem.Run();
    }
}
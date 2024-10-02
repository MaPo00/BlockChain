using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Force.Crc32;

 
var crc = new CRC32Hash();
var data = Enumerable.Range(0, 10).ToArray();
string parentHash = null!;

int? prevData = null;

var blockchain = new List<Block>();
foreach (var item in data)
{
 var block = new Block(parentHash, item.ToString());
 blockchain.Add(block);
 Console.WriteLine(block);
 parentHash = crc.GetHash(block.ParentHash + block.Data);

}

data[2] = 41; 


Console.Write("");
record Block(string ParentHash, string Data);

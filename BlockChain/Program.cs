using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Force.Crc32;

 
var crc = new CRC32Hash();
var data = Enumerable.Range(0, 10).ToArray();

var builder = new BlockchainBuilder(crc, null);
var blockchain = new Blockchain(crc);
foreach (var item in data)
{
    var block = builder.AddBlock(item.ToString());
    if (item == 7)
        block = block with { ParentHash = "gsgkosm" };
    blockchain.AddBlock(block);
}





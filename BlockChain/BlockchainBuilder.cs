record Block(string ParentHash, string Data, string Hash);

class BlockchainBuilder
{
    private readonly IHashFunction _hashFunction;
    private  string _tail;
    public BlockchainBuilder(IHashFunction hashFunction, string tail)
    {
        _hashFunction = hashFunction;
        _tail = tail;
    }

    public Block AddBlock(string data)
    {
        
        var hash = _hashFunction.GetHash(_tail + data);
        var block = new Block(_tail, data, hash);
        _tail = hash;
        return block;
    } 
}
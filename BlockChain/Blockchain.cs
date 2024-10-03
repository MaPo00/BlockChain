using System.Collections;

class Blockchain : IEnumerable<Block>
{   
    private readonly IHashFunction _hashFunction;
    private readonly List<Block> _blocks = new();

    public Blockchain(IHashFunction hashFunction)
    {
        _hashFunction = hashFunction;
    }

    public void AddBlock(Block block)
    {
        var tail = _blocks.LastOrDefault();
        if (tail == null || block.ParentHash == tail.Hash)
        {
            var expectedHash = _hashFunction.GetHash(block.Data + block.ParentHash);
            if (expectedHash == block.Hash)
            {
                _blocks.Add(block);
            }
            else
            {
                throw new ApplicationException($"Block Hash is invalid. Expected: {expectedHash}, Got: {block.Hash}");
            }
        }
        else
        {
            throw new ApplicationException($"Parent hash {block.ParentHash} is incorrect");
        }
    }

    public IEnumerator<Block> GetEnumerator()
    {
        return _blocks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
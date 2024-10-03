public class MerkleTree
{
    private readonly IHashFunction _hashFunction;
    private readonly List<string> _leaves;

    public MerkleTree(IHashFunction hashFunction)
    {
        _hashFunction = hashFunction;
        _leaves = new List<string>();
    }

    public void AddLeaf(string data)
    {
        _leaves.Add(data);
    }

    public string CalculateRoot()
    {
        return CalculateRoot(_leaves);
    }

    private string CalculateRoot(List<string> nodes)
    {
        if (nodes.Count == 0) return string.Empty;
        if (nodes.Count == 1) return _hashFunction.GetHash(nodes[0]);

        var newLevel = new List<string>();
        for (int i = 0; i < nodes.Count; i += 2)
        {
            var left = nodes[i];
            var right = (i + 1 < nodes.Count) ? nodes[i + 1] : left;
            var combined = _hashFunction.GetHash(left + right);
            newLevel.Add(combined);
        }

        return CalculateRoot(newLevel);
    }

    public void Clear()
    {
        _leaves.Clear();
    }
}
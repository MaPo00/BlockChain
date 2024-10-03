public class VotingSystem
{
    private readonly List<Candidate> candidates = new List<Candidate>();
    private readonly List<Vote> votes = new List<Vote>();
    private readonly Blockchain blockchain;
    private readonly IHashFunction hashFunction;
    private readonly MerkleTree merkleTree;
    private string initialMerkleRoot;

    public VotingSystem()
    {
        hashFunction = new CRC32Hash();
        blockchain = new Blockchain(hashFunction);
        
        var genesisBlock = new Block(string.Empty, "Генезис-блок системи голосування", hashFunction.GetHash("Генезис-блок системи голосування"));
        blockchain.AddBlock(genesisBlock);
        
        merkleTree = new MerkleTree(hashFunction);
    }
    public void Run()
    {
        SetupCandidates();
        SetupVoters();
        InitializeMerkleTree();
        ShowMenu();
    }
    private void InitializeMerkleTree()
    {
        merkleTree.Clear();
        foreach (var block in blockchain)
        {
            merkleTree.AddLeaf(block.Data);
        }
        initialMerkleRoot = merkleTree.CalculateRoot();
    }
    private void SetupCandidates()
    {
        Console.Write("Введіть кількість кандидатів: ");
        int candidateCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < candidateCount; i++)
        {
            Console.Write($"Введіть ім'я кандидата {i + 1}: ");
            string name = Console.ReadLine();
            candidates.Add(new Candidate { Name = name, Votes = 0 });
        }

        Console.WriteLine("Список кандидатів:");
        foreach (var candidate in candidates)
        {
            Console.WriteLine(candidate.Name);
        }
    }

    private void SetupVoters()
    {
        Console.Write("Введіть кількість учасників голосування: ");
        int voterCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < voterCount; i++)
        {
            Console.Write($"Учасник {i + 1}, за кого ви голосуєте? ");
            string candidateName = Console.ReadLine();
            
            var candidate = candidates.FirstOrDefault(c => c.Name == candidateName);
            if (candidate != null)
            {
                candidate.Votes++;
                var vote = new Vote { VoterId = i, CandidateName = candidateName };
                votes.Add(vote);
                var voteData = $"Voter{i}:{candidateName}";
                var parentHash = blockchain.Last().Hash;
                var blockData = voteData + parentHash;
                var blockHash = hashFunction.GetHash(blockData);
                var block = new Block(parentHash, voteData, blockHash);
                blockchain.AddBlock(block);
                Console.WriteLine("Голос враховано.");
            }
            else
            {
                Console.WriteLine("Такого кандидата немає. Голос не враховано.");
            }
        }
    }

    private void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Переглянути всі дані голосування");
            Console.WriteLine("2. Перевірка цілісності блокчейна");
            Console.WriteLine("3. Результат голосування");
            Console.WriteLine("4. Вихід");

            Console.Write("Виберіть дію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowVotingData();
                    break;
                case "2":
                    CheckBlockchainIntegrity();
                    break;
                case "3":
                    ShowVotingReport();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте знову");
                    break;
            }
        }
    }

    private void ShowBlockchainHashes()
    {
        Console.WriteLine("Хеші всіх блоків:");
        foreach (var block in blockchain)
        {
            Console.WriteLine($"Дані: {block.Data}, Хеш: {block.Hash}");
        }
    }

    

    private void CheckBlockchainIntegrity()
    {
        bool isValid = true;
        Block previousBlock = null;

        foreach (var block in blockchain)
        {
            if (previousBlock != null && block.ParentHash != previousBlock.Hash)
            {
                isValid = false;
                Console.WriteLine($"Порушення цілісності виявлено в блоці: {block.Data}");
                break;
            }
            previousBlock = block;
        }

        // Перевірка деревом Меркла
        var currentMerkleRoot = merkleTree.CalculateRoot();

        if (currentMerkleRoot != initialMerkleRoot)
        {
            isValid = false;
            Console.WriteLine("Виявлено зміни в даних за допомогою дерева Меркла.");
        }

        if (isValid)
        {
            Console.WriteLine("Блокчейн не порушено. Всі дані вірні.");
        }
        else
        {
            Console.WriteLine("Блокчейн порушено. Дані були змінені.");
        }
    }
    private void ShowVotingData()
    {
        Console.WriteLine("Дані голосування:");
        foreach (var block in blockchain)
        {
            Console.WriteLine($"Дані: {block.Data}, Хеш: {block.Hash}");
        }
    }
    private void ShowVotingReport()
    {
        Console.WriteLine("Результат голосування:");
        foreach (var candidate in candidates)
        {
            Console.WriteLine($"{candidate.Name}: {candidate.Votes} голосів");
        }
    }
}
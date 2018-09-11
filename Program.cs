using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WordFinder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await File.WriteAllTextAsync("words.txt",
                await new HttpClient().GetStringAsync("https://github.com/dwyl/english-words/raw/master/words.txt"));

            var validWords = (await File.ReadAllLinesAsync("words.txt"))
                .Select(x =>
                {
                    var w = x.ToLowerInvariant();
                    return (
                        w,
                        p: w.Sum(y => y - 96),
                        m: $"{(string.Join("+", w.ToCharArray().Select(p => (p - 96).ToString()).ToArray()))} = 100"
                    );
                })
                .Where(x => !x.w.Any(y => y < 97 || y > 122));

            var matchedWords = validWords.Where(x => x.p == 100);

            matchedWords.ToList().ForEach(x => Console.WriteLine($"{x.w} {x.m}"));

            Console.WriteLine($"Total: {matchedWords.Count()}/{validWords.Count()}");
            Console.Read();
        }
    }
}

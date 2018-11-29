using System.Collections.Generic;
using System.Linq;

namespace Bowling
{
    public class Frame
    {
        private readonly int[] pins;
        protected Frame next = null;

        public Frame(params int[] pins) => this.pins = pins;

        public void AddNext(Frame frame)
        {
            if (next != null)
            {
                next.AddNext(frame);
                return;
            }
            this.next = frame;
        }

        public IEnumerable<int> GetRolls()
        {
            foreach(var val in pins) 
                yield return val;
            foreach(var val in next?.GetRolls() ?? new int[0])
                yield return val;
        }

        public virtual int Score() => pins.Sum() + (next?.Score() ?? 0);
    }

    public class AllKnockedDown : Frame
    {
        public AllKnockedDown(params int[] pins) : base(pins) { }

        public override int Score() => GetRolls().Take(3).Sum() + (next?.Score() ?? 0);
    }

    public class Game : Frame
    {
        public Game(string input)
        {
            input.Replace('-', '0').Split(" ").ToList().ForEach(s => AddNext(Parser.Game.Parse(s)));
        }
    }

    public abstract class Parser
    {
        static public Parser Game { get;  }
        private Parser next = DefaultParser.Instance;

        static Parser()
        {
            Game = new StrikeParser();
            Game.AddNext(new SpareParser());
        }
        public Frame Parse(string input)
        {
            if (Applicable(input))
                return InternalParse(input);
            return next.Parse(input);
        }

        public void AddNext(Parser next) => this.next = next;
        internal abstract Frame InternalParse(string input);
        protected abstract bool Applicable(string input);
    }

    public class StrikeParser : Parser
    {
        protected override bool Applicable(string input) => input.Contains("x");

        internal override Frame InternalParse(string input)
        {
            var numbers = input.ToCharArray().Select(c => int.Parse((new string(new char[] {c}).Replace("x", "10")))).ToArray();
            return new AllKnockedDown(numbers);
        }
    }

    public class SpareParser : Parser
    {
        protected override bool Applicable(string input) => input.Contains('/');

        internal override Frame InternalParse(string input)
        {
            var numbers = input.Split("/").Where(s => s != "").Select(s => int.Parse(s)).ToArray();
            if (numbers.Length == 1)
                return new AllKnockedDown(numbers[0], 10-numbers[0]);
            else
                return new AllKnockedDown(numbers[0], 10-numbers[0], numbers[1]);
        }
    }

    internal class DefaultParser : Parser
    {
        internal static readonly Parser Instance = new DefaultParser();

        private DefaultParser() {}

        protected override bool Applicable(string input) => true;

        internal override Frame InternalParse(string input)
        {
            var numbers = input.ToCharArray().Select(c => int.Parse(new string(new char[] {c}))).ToArray();
            return new Frame(numbers);

        }
    }
}
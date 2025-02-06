
using Compare.AI.Raw.cs;

namespace Xll.Ai.Realtime.View // Ensure this namespace matches your project structure
{
    public enum AI_Engine
    {
        OpenAI_4o,
        OpenAI_o1,
        Perplexity,
        Llama,
        OpenAI_o1_4o,
    }
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine(Banner.Generate(" AI  "));
            Console.WriteLine(Banner.Generate(" Temporaral"));
            Console.WriteLine(Banner.Generate(" Cache"));
            bool runAll = false;

            if (runAll)
            {
                RunAll();
            }
            else
            { 
                AI_Engine aI_Engine = AI_Engine.OpenAI_o1_4o;

                Console.WriteLine("Running " + aI_Engine.ToString());

                switch (aI_Engine)
                {
                    case AI_Engine.OpenAI_o1_4o: OpenAI.o1.OpenAI4o.TemporalAsOfShowcase.Program_OpenAI_o1_4o.Main_OpenAI_o1_4o(null); break;
                    case AI_Engine.OpenAI_4o:    OpenAI4o.TemporalAsOfShowcase.Program_OpenAI_4o.Main_OpenAI_4o(); break;
                    case AI_Engine.OpenAI_o1:    OpenAI.o1.TemporalAsOfShowcase.Program_OpenAI_o1.Main_OpenAI_o1(null); break;
                    case AI_Engine.Perplexity:   Perplexity.TemporalAsOfShowcase.Program_Perplexity.Main_Perplexity(); break;
                    case AI_Engine.Llama:        Llama.TemporalAsOfShowcase.Program_Llama.Main_Llama(null); break;
        
                }
            }
        }

        private static void RunAll()
        {
            OpenAI4o.TemporalAsOfShowcase.Program_OpenAI_4o.Main_OpenAI_4o();
            OpenAI.o1.TemporalAsOfShowcase.Program_OpenAI_o1.Main_OpenAI_o1(null);
            Perplexity.TemporalAsOfShowcase.Program_Perplexity.Main_Perplexity();
            Llama.TemporalAsOfShowcase.Program_Llama.Main_Llama(null);
            OpenAI.o1.OpenAI4o.TemporalAsOfShowcase.Program_OpenAI_o1_4o.Main_OpenAI_o1_4o(null);
        }

    }
}


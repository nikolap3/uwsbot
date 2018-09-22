using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace consoleDOTnetcore
{
    /// <summary>
    /// Contains methods for counting words.
    /// </summary>
    public static class WordCounting
    {
        /// <summary>
        /// Count words with Regex.
        /// </summary>
        public static int CountWords1(string s, string l)
        {
            //l=  @"[\S]+"
            MatchCollection collection = Regex.Matches(s, l);
            return collection.Count;
        }

        /// <summary>
        /// Count word with loop and character tests.
        /// </summary>
        public static int CountWords2(string s)
        {
            int c = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i - 1]) == true)
                {
                    if (char.IsLetterOrDigit(s[i]) == true ||
                        char.IsPunctuation(s[i]))
                    {
                        c++;
                    }
                }
            }
            if (s.Length > 2)
            {
                c++;
            }
            return c;
        }
    }

    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;
            string token = "NDgxMTg4NzgxMzI0NzYzMTM4.Dl2FMQ.73-QfXwcU4a-o3IlcmNCFmRUdgE"; // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            SocketUser socketUser;
            socketUser = message.Author;
            string user = socketUser.Username;
            ulong iD = socketUser.Id;
            UInt16 discriminator = socketUser.DiscriminatorValue;
            string newmsg = "";
            string msg = message.Content;
            if (msg.StartsWith("u!team"))
            {
                msg = msg.Replace("u!team", " ");
                int n = WordCounting.CountWords1(msg, "@") + 1;
                string[] bmsg1;
                string[] msg1 = new string[0];
                bmsg1 = Regex.Split(msg, "<");

                for (int i = 1; i < bmsg1.Length; i++)
                {
                    //bmsg1[i-1] = "<" + bmsg1[i-1].ToString();
                    Array.Resize(ref msg1, msg1.Length + 1);
                    //Console.WriteLine((msg1.Length).ToString());
                    msg1[i - 1] = "<" + bmsg1[i].ToString();
                    Console.WriteLine((msg1.Length).ToString() + " tu je " + msg1[msg1.Length - 1]);
                }

                new Random().Shuffle(msg1);

                int j = msg1.Length;//3
                for (int i = 0; i < ((msg1.Length) / 2); i++)
                {
                    //ja i fred, mac i shufflebot 0 i 3,1 i 2
                    j--;//1
                    if (j == i)//1)=1
                    {
                        await message.Channel.SendMessageAsync((msg1[i].ToString() + " is alone").ToString());
                        break;
                    }
                    else if (i - j == 1)
                    {
                        newmsg = newmsg + (msg1[i].ToString() + " is alone").ToString();
                        break;
                    }
                    //Console.WriteLine(i.ToString());
                    newmsg = newmsg + ("Team " + (i + 1).ToString() + ": " + msg1[i].ToString() + " and " + msg1[j].ToString() + "\n").ToString();

                }



                await message.Channel.SendMessageAsync(newmsg);



                /*
                   //for (int i = 0; i < n; i++) Console.WriteLine(msg1[i].ToString());
                   //await message.Channel.SendMessageAsync(msg1[0]+"=0 and n="+msg1[(msg1.Length-1)]);
                   This is for testing purposes

                */
            }
            else if (msg.StartsWith("!countdown"))
            {
                msg = msg.Replace("!countdown", " ");
                for (int i = int.Parse(msg); i > 0; i--)
                {
                    await Task.Delay(2000);
                    await message.Channel.SendMessageAsync(i.ToString());
                }
            }
            else if (msg.StartsWith("u!coin"))
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 2);
                msg = msg.Replace("u!coin", " ");
                if (int.TryParse(msg, out int number)) if (int.Parse(msg) > 333)
                    {
                        newmsg = newmsg + "I cant do more than 333 coin flips";
                        await message.Channel.SendMessageAsync(newmsg.ToString());
                    }
                    else
                    {
                        //Random random = new Random();
                        //int randomNumber = random.Next(0, 2);
                        for (int i = 0; i < number - 1; i++)
                        {
                            //Random random = new Random();
                            randomNumber = random.Next(0, 2);
                            if (randomNumber == 1) msg = "Heads";
                            else msg = "Tails";
                            newmsg = newmsg + msg + "\n";
                            //await message.Channel.SendMessageAsync(msg.ToString());
                        }
                        randomNumber = random.Next(0, 2);
                        if (randomNumber == 1) msg = "Heads";
                        else msg = "Tails";
                        newmsg = newmsg + msg + "\n";
                        //await message.Channel.SendMessageAsync(msg.ToString());

                        await message.Channel.SendMessageAsync(newmsg.ToString());
                    }
                else
                {
                    if (randomNumber == 1) msg = "Heads";
                    else msg = "Tails";
                    newmsg = newmsg + msg + "\n";
                    await message.Channel.SendMessageAsync(msg.ToString());
                }
            }
            else if (msg.StartsWith("u!dice"))
            {
                string msg1 = "";
                int min, max;
                Random random = new Random();
                int randomNumber = random.Next(0, 2);
                msg = msg.Replace("u!dice", "");
                msg1 = Regex.Match(msg, @"-?\d+").Value;
                max = int.Parse(msg1);//number;
                var regex = new Regex(Regex.Escape(max.ToString()));
                msg = regex.Replace(msg,"", 1);
                Console.WriteLine("msg je " + msg);
                if (int.TryParse(msg, out int number2)) min = number2;
                else min = 1;
                //if (min < 0) min = -1 * min;
                //if (max < 0) max = -1 * max;
                if (min == max) Console.WriteLine("Min i Max su isti");
                else if(min > max) { int klog = min; min = max; max = klog; }
                Console.WriteLine("Min je "+min+" ,a max je "+max);
                randomNumber = random.Next(min, max+1);
                await message.Channel.SendMessageAsync("<@"+iD+"> "+" rolled "+randomNumber.ToString());
            }
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
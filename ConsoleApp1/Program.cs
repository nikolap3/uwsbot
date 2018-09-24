using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace consoleDOTnetcore
{
    public static class Pings
    {
        public static string[] Split(string msg)
        {
            string[] bmsg1;
            string[] msg1 = new string[0];
            bmsg1 = Regex.Split(msg, "<");

            for (int i = 1; i < bmsg1.Length; i++)
            {
                Array.Resize(ref msg1, msg1.Length + 1);
                msg1[i - 1] = "<" + bmsg1[i].ToString();
            }
            return msg1;
        }
    }
    public static class WordCounting
    {
        /// <summary>
        /// Count words with Regex.
        /// </summary>
        public static int CountWords1(string s, string l)
        {
            MatchCollection collection = Regex.Matches(s, l);
            return collection.Count;
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

                string[] msg1 = Pings.Split(msg);

                new Random().Shuffle(msg1);

                int j = msg1.Length;
                for (int i = 0; i < ((msg1.Length) / 2); i++)
                {
                    j--;
                    if (j == i)
                    {
                        await message.Channel.SendMessageAsync((msg1[i].ToString() + " is alone").ToString());
                        break;
                    }
                    else if (i - j == 1)
                    {
                        newmsg = newmsg + (msg1[i].ToString() + " is alone").ToString();
                        break;
                    }
                    newmsg = newmsg + ("Team " + (i + 1).ToString() + ": " + msg1[i].ToString() + " and " + msg1[j].ToString() + "\n").ToString();

                }

                await message.Channel.SendMessageAsync(newmsg);

            }
            else if (msg.StartsWith("u!countdown"))
            {
                msg = msg.Replace("u!countdown", " ");
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
                        for (int i = 0; i < number - 1; i++)
                        {
                            randomNumber = random.Next(0, 2);
                            if (randomNumber == 1) msg = "Heads";
                            else msg = "Tails";
                            newmsg = newmsg + msg + "\n";
                        }
                        randomNumber = random.Next(0, 2);
                        if (randomNumber == 1) msg = "Heads";
                        else msg = "Tails";
                        newmsg = newmsg + msg + "\n";

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
                msg = msg.Replace("u!dice ", "");
                msg1 = Regex.Match(msg, @"-?\d+").Value;
                max = int.Parse(msg1);//number;
                var regex = new Regex(Regex.Escape(max.ToString()));
                msg = regex.Replace(msg,"", 1);
                Console.WriteLine("msg je " + msg);
                if (int.TryParse(msg, out int number2)) min = number2;
                else min = 1;
                if (min == max) Console.WriteLine("Min i Max su isti");
                else if(min > max) { int klog = min; min = max; max = klog; }
                Console.WriteLine("Min je "+min+" ,a max je "+max);
                randomNumber = random.Next(min, max+1);
                await message.Channel.SendMessageAsync("<@"+iD+"> "+" rolled "+randomNumber.ToString());
            }
            else if (msg.StartsWith("u!rps"))
            {
                msg = msg.Replace("u!rps ", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                int[] threw = new int[names.Length];


                for (int i = 0; i < names.Length; i++)
                {
                    randomNumber = random.Next(0, 3);
                    switch (randomNumber)
                    {
                        case 0:
                            threw[i]=randomNumber;
                            newmsg = newmsg + names[i] + ": :v: \n";
                            break;
                        case 1:
                            threw[i] = randomNumber;
                            newmsg = newmsg +names[i] + ": :fist: \n";
                            break;
                        case 2:
                            threw[i] = randomNumber;
                            newmsg = newmsg + names[i] + ": :hand_splayed: \n";
                            break;
                    }
                    Console.WriteLine((i+""+ threw[i]).ToString());
                }

                int win = 0;
                int winnie = 0;
                Console.WriteLine(("\nlenght je"+names.Length).ToString());
                for (int i = 0; i < names.Length; i++)
                {
                    win = 0;
                    for (int j = 0; j < names.Length ; j++)
                    {
                        if (i == j) continue;
                        if (threw[i] == 1 && threw[j] == 0)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i+1;
                        }
                        else if (threw[i] == 2 && threw[j] == 1)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i+1;
                        }
                        else if (threw[i] == 0 && threw[j] == 2)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i+1;
                        }

                    }
                }
                Console.WriteLine(("\ntrenutni winnie je " + winnie).ToString());
                if (winnie != 0) newmsg = newmsg + names[winnie-1] + " WINS";
                else if (winnie == 0 && win != 0) newmsg = newmsg + "NO ONE WINS";
                else newmsg = newmsg + "NO RESULT";
                await message.Channel.SendMessageAsync(newmsg);
            }
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

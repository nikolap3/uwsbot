using Discord;
using Discord.Commands;
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
        public static string[] add(string[] array, string url)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = url;
            return array;
        }
    }
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
            string token = "NDgxMTg4NzgxMzI0NzYzMTM4.Dl2FMQ.73-QfXwcU4a-o3IlcmNCFmRUdgE"; //uws bot token Remember to keep this private!
            //string token = "NDk0NTIwMDY0NDIzNDkzNjMy.Do0ttw.zagIXZTyWMeICcK1nCTz2nFoDYE";//test bot
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage context)
        {
            SocketUser socketUser;
            socketUser = context.Author;
            MessageSource messageType = context.Source;
            ISocketMessageChannel socketChannel = context.Channel;
            IUser iuser;
            string user = socketUser.Username;
            ulong iD = socketUser.Id;
            UInt16 discriminator = socketUser.DiscriminatorValue;
            string newmsg = "";
            string msg = context.Content;
            if (msg.StartsWith("u!team "))
            {
                msg = msg.Replace("u!team ", " ");
                int n = WordCounting.CountWords1(msg, "@") + 1;

                string[] msg1 = Pings.Split(msg);

                new Random().Shuffle(msg1);

                int j = msg1.Length;//3
                for (int i = 0; i < ((msg1.Length) / 2); i++)
                {
                    j--;
                    if (j == i)
                    {
                        await context.Channel.SendMessageAsync((msg1[i].ToString() + " is alone").ToString());
                        break;
                    }
                    else if (i - j == 1)
                    {
                        newmsg = newmsg + (msg1[i].ToString() + " is alone").ToString();
                        break;
                    }
                    newmsg = newmsg + ("Team " + (i + 1).ToString() + ": " + msg1[i].ToString() + " and " + msg1[j].ToString() + "\n").ToString();

                }



                await context.Channel.SendMessageAsync(newmsg);
            }
            else if (msg.StartsWith("u!countdown "))
            {
                msg = msg.Replace("u!countdown", " ");
                for (int i = int.Parse(msg); i > 0; i--)
                {
                    await Task.Delay(2000);
                    await context.Channel.SendMessageAsync(i.ToString());
                }
                await context.Channel.SendMessageAsync(messageType.ToString());
            }
            else if (msg.StartsWith("u!coin"))
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 2);
                msg = msg.Replace("u!coin", " ");
                if (int.TryParse(msg, out int number)) if (int.Parse(msg) > 333)
                    {
                        newmsg = newmsg + "I cant do more than 333 coin flips";
                        await context.Channel.SendMessageAsync(newmsg.ToString());
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

                        await context.Channel.SendMessageAsync(newmsg.ToString());
                    }
                else
                {
                    if (randomNumber == 1) msg = "Heads";
                    else msg = "Tails";
                    newmsg = newmsg + msg + "\n";
                    await context.Channel.SendMessageAsync(msg.ToString());
                }
            }
            else if (msg.StartsWith("u!dice "))
            {
                string msg1 = "";
                int min, max;
                Random random = new Random();
                int randomNumber = random.Next(0, 2);
                msg = msg.Replace("u!dice", "");
                msg1 = Regex.Match(msg, @"-?\d+").Value;
                max = int.Parse(msg1);
                var regex = new Regex(Regex.Escape(max.ToString()));
                msg = regex.Replace(msg,"", 1);
                Console.WriteLine("msg je " + msg);
                if (int.TryParse(msg, out int number2)) min = number2;
                else min = 1;
                if (min == max) Console.WriteLine("Min i Max su isti");
                else if(min > max) { int klog = min; min = max; max = klog; }
                Console.WriteLine("Min je "+min+" ,a max je "+max);
                randomNumber = random.Next(min, max+1);
                await context.Channel.SendMessageAsync("<@"+iD+"> "+" rolled "+randomNumber.ToString());
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
                            threw[i] = randomNumber;
                            newmsg = newmsg + names[i] + ": :v: \n";
                            break;
                        case 1:
                            threw[i] = randomNumber;
                            newmsg = newmsg + names[i] + ": :fist: \n";
                            break;
                        case 2:
                            threw[i] = randomNumber;
                            newmsg = newmsg + names[i] + ": :hand_splayed: \n";
                            break;
                    }
                    Console.WriteLine((i + "" + threw[i]).ToString());
                }

                int win = 0;
                int winnie = 0;
                Console.WriteLine(("\nlenght je" + names.Length).ToString());
                for (int i = 0; i < names.Length; i++)
                {
                    win = 0;
                    for (int j = 0; j < names.Length; j++)
                    {
                        if (i == j) continue;
                        if (threw[i] == 1 && threw[j] == 0)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i + 1;
                        }
                        else if (threw[i] == 2 && threw[j] == 1)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i + 1;
                        }
                        else if (threw[i] == 0 && threw[j] == 2)
                        {
                            win++;
                            if (win == names.Length - 1) winnie = i + 1;
                        }

                    }
                }
                Console.WriteLine(("\ntrenutni winnie je " + winnie).ToString());
                if (winnie != 0) newmsg = newmsg + names[winnie - 1] + " WINS";
                else if (winnie == 0 && win != 0) newmsg = newmsg + "NO ONE WINS";
                else newmsg = newmsg + "NO RESULT";
                await context.Channel.SendMessageAsync(newmsg);
            }
            else if (msg.StartsWith("u!slap "))
            {
                string title = "";
                msg = msg.Replace("u!slap", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
                    title = user + " slaps themself";
                else
                {
                    names[0] = names[0].Remove(0, 2);
                    names[0] = names[0].Replace(">", "");
                    ulong.TryParse(names[0], out ulong result);
                    iuser = await socketChannel.GetUserAsync(result);
                    title = user + " slaps " + iuser.Username + " ";
                }
                string[] gif = new string[0];
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147468787843092/original.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147323169865728/tumblr_mflza5vE4o1r72ht7o2_400.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147192068767744/never_ending_bitch_slap_by_yindragon-d4kiubr.gif");
                randomNumber = random.Next(0, gif.Length);
                Console.WriteLine(gif.Length.ToString());
                var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
                var embed = builder.Build();
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }
            else if (msg.StartsWith("u!help"))
            {
                newmsg = "```u!countdown <number> - counts down from a number \n u!rps < player 1 > < player 2 >...... [player n] - plays a rock, paper, scissors game with all the players\n u!coin[number] - flips a coin x times\n u!team < person 1 >< person 2 > ..........[person n] - makes teams of 2 with all the people mentioned\n u!dice < number 1 >[number 2] - randomly chooses a number from 1 to number1 or a number in between number1 and number2 \n u!slap <person1> - slaps \n <something> -are necessary inputs \n  [something] -are optional inputs```";
                await context.Channel.SendMessageAsync(newmsg);
            }
            
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

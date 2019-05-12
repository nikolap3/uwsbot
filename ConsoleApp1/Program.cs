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
        public async Task Countdown(string msg, SocketMessage context)
        {
            msg = msg.Replace("u!countdown", " ");
            for (int i = int.Parse(msg); i > 0; i--)
            {
                await Task.Delay(1000);
                //await context.Channel.SendMessageAsync(i.ToString());
            }
            await context.Channel.SendMessageAsync("it has been " + int.Parse(msg).ToString());
        }

        public async Task Coin(string msg, SocketMessage context)
        {
            Random random = new Random();
            string newmsg = "";
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

        public async Task Teaming(string msg, SocketMessage context)
        {
            string newmsg = "";

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
                    newmsg += (msg1[i].ToString() + " is alone").ToString();
                    break;
                }
                newmsg += ("Team " + (i + 1).ToString() + ": " + msg1[i].ToString() + " and " + msg1[j].ToString() + "\n").ToString();

            }

            await context.Channel.SendMessageAsync(newmsg);
        }

        public async Task RPS(string msg, SocketMessage context)
        {
            string newmsg = "";

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

        public async Task Diceroll(string msg, SocketMessage context)
        {
            string msg1 = "";
            int min, max;
            Random random = new Random();
            int randomNumber = random.Next(0, 2);
            msg = msg.Replace("u!dice", "");
            msg1 = Regex.Match(msg, @"-?\d+").Value;
            max = int.Parse(msg1);
            var regex = new Regex(Regex.Escape(max.ToString()));
            msg = regex.Replace(msg, "", 1);
            Console.WriteLine("msg je " + msg);
            if (int.TryParse(msg, out int number2)) min = number2;
            else min = 1;
            if (min == max) Console.WriteLine("Min i Max su isti");
            else if (min > max) { int klog = min; min = max; max = klog; }
            Console.WriteLine("Min je " + min + " ,a max je " + max);
            randomNumber = random.Next(min, max + 1);
            await context.Channel.SendMessageAsync("<@" + context.Author.Id + "> " + " rolled " + randomNumber.ToString());
        }

        public async Task Slap(string msg, SocketMessage context, string user, ulong iD, ISocketMessageChannel socketChannel)
        {
            IUser iuser;
            string title = "";
            msg = msg.Replace("u!slap", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if(names.Length>0) names[0] = names[0].Replace("!", "");
            if (names.Length == 0 || names[0] == ("<@" + (iD).ToString() + ">").ToString())
                title = user + " slaps themself";
            else
            {
                Console.WriteLine("id pre prerade je " + names[0]);
                names[0] = names[0].Remove(0, 2);
                names[0] = names[0].Replace(">", "");
                ulong.TryParse(names[0], out ulong result);
                iuser = await socketChannel.GetUserAsync(result);
                Console.WriteLine(result.ToString());
                title = user + " slaps " + iuser.Username + " ";
            }
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494144313404555274/giphy.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147027064717322/3b3c291b732c757fc2a9d0f18d34402e37349b73_hq.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147468787843092/original.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147323169865728/tumblr_mflza5vE4o1r72ht7o2_400.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147192068767744/never_ending_bitch_slap_by_yindragon-d4kiubr.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await context.Channel.SendMessageAsync("", embed: embed);
        }

        public async Task Kick(string msg, SocketMessage context, string user, ulong iD, ISocketMessageChannel socketChannel)
        {
            IUser iuser;
            string title = "";
            msg = msg.Replace("u!kick", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if (names.Length > 0) names[0] = names[0].Replace("!", "");
            if (names.Length == 0 || names[0] == ("<@" + (iD).ToString() + ">").ToString())
                title = user + " kicks themself";
            else
            {
                names[0] = names[0].Remove(0, 2);
                names[0] = names[0].Replace(">", "");
                ulong.TryParse(names[0], out ulong result);
                iuser = await socketChannel.GetUserAsync(result);
                title = user + " kicks " + iuser.Username + " ";
            }
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562631588511744/kick5.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562673619369994/kick1.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562720549568512/kick2.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494577333576138763/kick3.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562816133431296/kick4.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await context.Channel.SendMessageAsync("", embed: embed);
        }

        public async Task Gaykiss(string msg, SocketMessage context, string user, ulong iD, ISocketMessageChannel socketChannel)
        {
            IUser iuser;
            string title = "";
            msg = msg.Replace("u!gaykiss", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if (names.Length > 0) names[0] = names[0].Replace("!", "");
            if (names.Length == 0 || names[0] == ("<@" + (iD).ToString() + ">").ToString())
                title = user + " kisses themself";
            else
            {
                names[0] = names[0].Remove(0, 2);
                names[0] = names[0].Replace(">", "");
                ulong.TryParse(names[0], out ulong result);
                iuser = await socketChannel.GetUserAsync(result);
                title = user + " gaykisses " + iuser.Username + " ";
            }
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494576499051986964/gk1.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494576517007933440/gk4.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494576517670633482/gk3.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494576520635875328/gk2.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494576524226330634/tumblr_inline_n67apaUGX81rgg4k4.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await context.Channel.SendMessageAsync("", embed: embed);
        }

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;
            //string token = "NDgxMTg4NzgxMzI0NzYzMTM4.Dl2FMQ.73-QfXwcU4a-o3IlcmNCFmRUdgE"; //uws bot token Remember to keep this private!
            string token = "NDk0NTIwMDY0NDIzNDkzNjMy.XNdCGA.CR7IBanH75B8GleLOZS8QDi9xrg";//test bot
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
            string user = socketUser.Username;
            ulong iD = socketUser.Id;
            UInt16 discriminator = socketUser.DiscriminatorValue;
            string newmsg = "";
            string msg = context.Content;
            if (msg.StartsWith("u!team "))
            {
                await Teaming(msg, context);
            }
            else if (msg.StartsWith("u!countdown "))
            {
                await Countdown(msg, context);
            }
            else if (msg.StartsWith("u!coin"))
            {
                await Coin(msg, context);
            }
            else if (msg.StartsWith("u!dice "))
            {
                await Diceroll(msg, context);
            }
            else if (msg.StartsWith("u!rps"))
            {
                await RPS(msg, context);
            }
            else if (msg.StartsWith("u!slap "))
            {
                await Slap(msg, context, user, iD, socketChannel);
            }
            else if (msg.StartsWith("u!kick "))
            {
                await Kick(msg, context, user, iD, socketChannel);
            }
            else if (msg.StartsWith("u!gaykiss "))
            {
                await Gaykiss(msg, context, user, iD, socketChannel);
            }
            else if (msg.StartsWith("u!help"))
            {
                newmsg = "```u!countdown <number> - counts down from a number \n\n"
                    + "u!rps < player 1 > < player 2 >...... [player n] - plays a rock, paper, scissors game with all the players\n\n"
                    + "u!coin[number] - flips a coin x times\n "
                    + "u!team < person 1 >< person 2 > ..........[person n] - makes teams of 2 with all the people mentioned\n\n"
                    + "u!dice < number 1 >[number 2] - randomly chooses a number from 1 to number1 or a number in between number1 and number2 \n\n"
                    + "u!slap <person1> - slaps\n\nu!gaykiss <person1> - gaykiss a person\n\n"
                    + "u!kick <person1> - kick a guy\n\n\n"
                    + "<something> -are necessary inputs \n[something] -are optional inputs```";
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

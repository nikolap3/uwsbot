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
            string token = Environment.GetEnvironmentVariable("Discord_Token"); //uws bot token Remember to keep this private
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage context)
        {
            static bool countd = false;
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
                countd=true;
                for (int i = int.Parse(msg); i > 0; i--)
                {
                    if(!countd)break;
                    await Task.Delay(2000);
                    await context.Channel.SendMessageAsync(i.ToString());
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
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494144313404555274/giphy.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147027064717322/3b3c291b732c757fc2a9d0f18d34402e37349b73_hq.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147468787843092/original.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147323169865728/tumblr_mflza5vE4o1r72ht7o2_400.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147192068767744/never_ending_bitch_slap_by_yindragon-d4kiubr.gif");
                randomNumber = random.Next(0, gif.Length);
                Console.WriteLine(gif.Length.ToString());
                var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
                var embed = builder.Build();
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }
            else if (msg.StartsWith("u!cuddle "))
            {
                string title = "";
                msg = msg.Replace("u!cuddle", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
                    title = user + " cuddles themself";
                else
                {
                    names[0] = names[0].Remove(0, 2);
                    names[0] = names[0].Replace(">", "");
                    ulong.TryParse(names[0], out ulong result);
                    iuser = await socketChannel.GetUserAsync(result);
                    title = user + " cuddles " + iuser.Username + " ";
                }
                string[] gif = new string[0];
                gif = Pings.add(gif, "https://media1.tenor.com/images/dc2bfb487be830983df6f8cc61e89368/tenor.gif?itemid=13797838");
                gif = Pings.add(gif, "https://media1.tenor.com/images/54e97e0cdeefea2ee6fb2e76d141f448/tenor.gif?itemid=11378437");
                gif = Pings.add(gif, "https://media1.tenor.com/images/88f1121f72f5fbfe4bc90c51cf82937f/tenor.gif?itemid=5047796");
                gif = Pings.add(gif, "https://media1.tenor.com/images/adeb030aaa5a2a3d16abdc58be4d1448/tenor.gif?itemid=11733535");
                gif = Pings.add(gif, "https://media1.tenor.com/images/4a211d5c5d076ad8795d8a82f9f01c29/tenor.gif?itemid=13221038");
                randomNumber = random.Next(0, gif.Length);
                Console.WriteLine(gif.Length.ToString());
                var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
                var embed = builder.Build();
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }
            else if (msg.StartsWith("u!hug "))
            {
                string title = "";
                msg = msg.Replace("u!hug", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
                    title = user + " hugs themself";
                else
                {
                    names[0] = names[0].Remove(0, 2);
                    names[0] = names[0].Replace(">", "");
                    ulong.TryParse(names[0], out ulong result);
                    iuser = await socketChannel.GetUserAsync(result);
                    title = user + " hugs " + iuser.Username + " ";
                }
                string[] gif = new string[0];
                gif = Pings.add(gif, "https://media.tenor.com/images/d5c635dcb613a9732cfd997b6a048f80/tenor.gif");
                gif = Pings.add(gif, "https://media1.tenor.com/images/42922e87b3ec288b11f59ba7f3cc6393/tenor.gif?itemid=5634630");
                gif = Pings.add(gif, "https://media1.tenor.com/images/1069921ddcf38ff722125c8f65401c28/tenor.gif?itemid=11074788");
                gif = Pings.add(gif, "https://media1.tenor.com/images/460c80d4423b0ba75ed9592b05599592/tenor.gif?itemid=5044460");
                gif = Pings.add(gif, "https://media1.tenor.com/images/44b4b9d5e6b4d806b6bcde2fd28a75ff/tenor.gif?itemid=9383138");
                gif = Pings.add(gif, "https://images-ext-2.discordapp.net/external/dwTWixM8K-ZizvgespNWYLLV3LtHY1J3pen3pq4IGGQ/https/i.giphy.com/rSNAVVANV5XhK.gif");
                gif = Pings.add(gif, "https://media.tenor.com/images/a756a73934fb6252bb9acf174d019c73/tenor.gif");
                gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/613686019077832704/613687991399088138/Friends-1.gif");
                gif = Pings.add(gif, "https://media1.tenor.com/images/5845f40e535e00e753c7931dd77e4896/tenor.gif?itemid=9920978");
                gif = Pings.add(gif, "https://media1.tenor.com/images/7ece0d6e9306763eeea5e0c5284a3528/tenor.gif?itemid=14106855");
                gif = Pings.add(gif, "https://media1.tenor.com/images/7db5f172665f5a64c1a5ebe0fd4cfec8/tenor.gif?itemid=9200935");
                gif = Pings.add(gif, "https://media1.tenor.com/images/cd321aa5d055a7e02b52eea806b9797c/tenor.gif?itemid=12861205");
                randomNumber = random.Next(0, gif.Length);
                Console.WriteLine(gif.Length.ToString());
                var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
                var embed = builder.Build();
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }
            else if (msg.StartsWith("u!pat "))
            {
                string title = "";
                msg = msg.Replace("u!pat", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
                    title = user + " pats themself";
                else
                {
                    names[0] = names[0].Remove(0, 2);
                    names[0] = names[0].Replace(">", "");
                    ulong.TryParse(names[0], out ulong result);
                    iuser = await socketChannel.GetUserAsync(result);
                    title = user + " pats " + iuser.Username + " ";
                }
                string[] gif = new string[0];
                gif = Pings.add(gif, "https://media1.tenor.com/images/f5176d4c5cbb776e85af5dcc5eea59be/tenor.gif?itemid=5081286");
                gif = Pings.add(gif, "https://media1.tenor.com/images/f330c520a8dfa461130a799faca13c7e/tenor.gif?itemid=13911345");
                gif = Pings.add(gif, "https://media1.tenor.com/images/54722063c802bac30d928db3bf3cc3b4/tenor.gif?itemid=8841561");
                gif = Pings.add(gif, "https://media1.tenor.com/images/282cc80907f0fe82d9ae1f55f1a87c03/tenor.gif?itemid=12018857");
                gif = Pings.add(gif, "https://media1.tenor.com/images/1e92c03121c0bd6688d17eef8d275ea7/tenor.gif?itemid=9920853");
                gif = Pings.add(gif, "https://media1.tenor.com/images/7938cdb8aa798486e2e2f1d997ea7797/tenor.gif?itemid=14816799");
                gif = Pings.add(gif, "https://media1.tenor.com/images/5466adf348239fba04c838639525c28a/tenor.gif?itemid=13284057");
                gif = Pings.add(gif, "https://media1.tenor.com/images/183ff4514cbe90609e3f286adaa3d0b4/tenor.gif?itemid=5518321");
                gif = Pings.add(gif, "https://media1.tenor.com/images/573c5aa94d1ab4ba9d32f369d35e1c8d/tenor.gif?itemid=14848813");
                gif = Pings.add(gif, "https://media1.tenor.com/images/8c1f6874db27c8227755a08b2b07740b/tenor.gif?itemid=10789367");
                randomNumber = random.Next(0, gif.Length);
                Console.WriteLine(gif.Length.ToString());
                var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
                var embed = builder.Build();
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }
            else if (msg.StartsWith("u!kick "))
            {
                string title = "";
                msg = msg.Replace("u!kick", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
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
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }/*
            else if (msg.StartsWith("u!gaykiss "))
            {
                string title = "";
                msg = msg.Replace("u!gaykiss", "");
                Random random = new Random();
                int randomNumber = random.Next(0, 3);
                string[] names = Pings.Split(msg);
                if (names.Length == 0 || names[0] == ("<@" + iD + ">").ToString())
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
                await context.Channel.SendMessageAsync(    "",    embed: embed);
            }*/
            else if (msg.StartsWith("u!help"))
            {
                newmsg = "```u!countdown <number> - counts down from a number \n\nu!rps < player 1 > < player 2 >...... [player n] - plays a rock, paper, scissors game with all the players\n\nu!coin[number] - flips a coin x times\n u!team < person 1 >< person 2 > ..........[person n] - makes teams of 2 with all the people mentioned\n\nu!dice < number 1 >[number 2] - randomly chooses a number from 1 to number1 or a number in between number1 and number2 \n\nu!slap <person1> - slaps\n\nu!gaykiss <person1> - gaykiss a person\n\nu!kick <person1> - kick a guy\n\n\n<something> -are necessary inputs \n[something] -are optional inputs```";
                await context.Channel.SendMessageAsync(newmsg);
            }
            else if(msg.StartWith("u!stop"))
            {
                countd = false;
                await context.Channel.SendMessageAsync("Set to stop");
            }
            
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

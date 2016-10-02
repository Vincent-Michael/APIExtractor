using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIExtractor
{
    public static class Program
    {
        static string apiKey = $"apikey={ Settings.APIKey }";
        private const string CommaSeparator = ", ";

        public static List<Spell> Spells = new List<Spell>();

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Console.WriteLine("Starting...");

            ReadSpellAPI();
            WriteAPISpellSQL();

            var endTime = DateTime.Now;
            var span = endTime.Subtract(startTime);
            Console.WriteLine($"{ TimeSpan.FromTicks(span.Ticks).ToString() }");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static void ReadSpellAPI()
        {
            Parallel.For(1, 250000, i =>
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string downloadString = "https://us.api.battle.net/wow/spell/";
                        string locale = $"locale={ Settings.Locale }";
                        string compiledString = $"{ downloadString }{ i }?{ apiKey }&{ locale }";
                        string value = webClient.DownloadString(compiledString);

                        Spell spellInfo = JsonConvert.DeserializeObject<Spell>(value);
                        Spells.Add(spellInfo);

                        Console.WriteLine($"( ID: { spellInfo.id }, Name: '{ spellInfo.name }' Icon: '{ spellInfo.icon }' Description: '{ spellInfo.description }' PowerCost: '{ spellInfo.powerCost }' CastTime: '{ spellInfo.castTime }'  Cooldown: '{ spellInfo.cooldown }')");
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
            });
        }

        public static void WriteAPISpellSQL()
        {
            StreamWriter sql = File.CreateText("spell_api.sql");
            string tableName = "spell_api";
            string[] fieldsName = { "id", "name", "icon", "description", "powerCost", "castTime", "cooldown" };

            sql.WriteLine(InsertBuild(tableName, fieldsName));

            var count = 0;
            StringBuilder query = new StringBuilder();

            foreach (Spell spellInfo in Spells.OrderBy(t => t.id))
            {
                query.Append($"({ spellInfo.id }, '{ EscapeString(spellInfo.name) }', '{ EscapeString(spellInfo.icon) }', '{ EscapeString(spellInfo.description) }', ");
                query.Append($"'{ EscapeString(spellInfo.powerCost) }', '{ EscapeString(spellInfo.castTime) }', '{ EscapeString(spellInfo.cooldown) }')");

                if (count < 500)
                    query.Append(",");
                else
                {
                    query.Append(";");
                    query.Append(InsertBuild(tableName, fieldsName));
                    query.Append(Environment.NewLine);
                    count = 0;
                }

                ++count;
                query.Append(Environment.NewLine);
            }

            query.ReplaceLast(',', ';');

            sql.WriteLine(query.ToString());
            sql.Close();
        }

        static public string InsertBuild(string tableName, string[] fieldsName)
        {
            StringBuilder query = new StringBuilder();

            query.Append("INSERT ");
            query.Append("INTO ");
            query.Append($"`{ tableName }`");

            query.Append(" (");

            foreach (var fieldName in fieldsName)
            {
                query.Append($"`{ fieldName }`");
                query.Append(CommaSeparator);
            }

            query.Remove(query.Length - CommaSeparator.Length, CommaSeparator.Length);
            query.Append(")");
            query.Append(" VALUES");

            return query.ToString();
        }

        public static void ReplaceLast(this StringBuilder str, char oldChar, char newChar)
        {
            for (var i = str.Length - 1; i > 0; i--)
                if (str[i] == oldChar)
                {
                    str[i] = newChar;
                    break;
                }
        }

        public static string EscapeString(string str)
        {
            if (str == null)
                return str;

            return MySqlHelper.DoubleQuoteString(str);
        }
    }
}


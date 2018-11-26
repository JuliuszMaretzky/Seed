﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Seed
{
    public static class Battle
    {
        public static List<Character> Garbage = new List<Character>();

        public static void Fight(Character attacker, Character defender, World world)
        {
            uint attackerDamage, defenderDamage;
            if (attacker.Strength >= 3 * defender.Armor)
            {
                CleanTheMess(defender, world);
            }
            else
            {
                ComputeDamage(attacker, defender, out attackerDamage, out defenderDamage);
                do
                {
                    defender.HP -= (int)attackerDamage;
                    attacker.HP -= (int)defenderDamage;
                } while (attacker.HP > 0 && defenderDamage > 0);
            }

            if (attacker.HP == 0)
            {
                CleanTheMess(attacker, world);
            }
            if (defender.HP == 0)
            {
                CleanTheMess(attacker, world);
            }

        }

        public static void Fight(Player player, Character foe, World world)
        {
            Random r = new Random();
            byte success;
            int foeStartFightHP = foe.HP, playerStartFightHP = player.HP;
            uint playerDamage, foeDamage, dmgGiven;
            ComputeDamage(player, foe, out playerDamage, out foeDamage);
            do
            {
                Service.DisplayStats(player);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"{foe.Name} HP:{foe.HP}");
                Console.ForegroundColor = ConsoleColor.White;
                success = (byte)(r.Next(0, 10) % 3);

                if (success > 0)
                {
                    dmgGiven = playerDamage - (uint)(Math.Pow(-1, success) * playerDamage);

                    if (dmgGiven > foeStartFightHP * 0.7)
                        Console.Write("Twój cios miażdży ");
                    else if (dmgGiven > foeStartFightHP * 0.6)
                        Console.Write("Twoje uderzenie dewastuje ");
                    else if (dmgGiven > foeStartFightHP * 0.5)
                        Console.Write("Twoje uderzenie masakruje ");
                    else if (dmgGiven > foeStartFightHP * 0.4)
                        Console.Write("Twoje trafienie grzmoci ");
                    else if (dmgGiven > foeStartFightHP * 0.3)
                        Console.Write("Twój kopniak tłucze ");
                    else if (dmgGiven > foeStartFightHP * 0.2)
                        Console.Write("Twój trafienie trzepie ");
                    else if (dmgGiven > foeStartFightHP * 0.1)
                        Console.Write("Twój plaskacz muska ");
                    else
                        Console.WriteLine("Twój piruet głaszcze ");
                    Console.WriteLine(foe.Name + "!");
                    foe.HP -= (int)dmgGiven;

                    if (foe.HP == 0)
                        break;
                }
                else
                {
                    Console.WriteLine("Nie trafiasz!");
                }

                success = (byte)(r.Next(0, 10) % 3);
                if (success > 0)
                {
                    dmgGiven = foeDamage - (uint)(Math.Pow(-1, success) * foeDamage);

                    if (dmgGiven > foeStartFightHP * 0.7)
                        Console.WriteLine($"{foe.Name} ciosem miażdży ciebie!");
                    else if (dmgGiven > playerStartFightHP * 0.6)
                        Console.Write($"Uderzenie {foe.Name} dewastuje cię!");
                    else if (dmgGiven > playerStartFightHP * 0.5)
                        Console.Write($"Uderzenie {foe.Name} masakruje cię!");
                    else if (dmgGiven > playerStartFightHP * 0.4)
                        Console.Write($"{foe.Name} trafia cię i grzmoci twój pysk!");
                    else if (dmgGiven > playerStartFightHP * 0.3)
                        Console.Write($"{foe.Name} tłucze cię kopniakiem!");
                    else if (dmgGiven > playerStartFightHP * 0.2)
                        Console.Write($"{foe.Name} trzepie cię w ucho!");
                    else if (dmgGiven > playerStartFightHP * 0.1)
                        Console.Write($"Obrywasz od {foe.Name} z plaskacza!");
                    else
                        Console.Write($"{foe.Name} ledwie cię muska!");
                    player.HP -= (int)dmgGiven;

                }
                else
                {
                    Console.WriteLine($"{foe.Name} nie trafia!");
                }

                System.Threading.Thread.Sleep(900);
            } while (player.HP > 0 && foe.HP > 0);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (player.HP == 0)
            {
                Console.WriteLine("NIE ŻYJESZ!");
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Zabiłeś {foe.Name}!");
                CleanTheMess(foe, world);
            }
        }

        public static void ComputeDamage(Character fighter1, Character fighter2, out uint fighter1Damage,
            out uint fighter2Damage)
        {
            if (fighter1.Strength >= fighter2.Armor)
            {
                fighter1Damage = (uint)(((fighter1.Strength - fighter2.Armor) * 0.05 + 1.00) *
                    fighter1.Damage);
            }
            else
            {
                fighter1Damage = (uint)((1.00 - ((fighter2.Armor - fighter1.Strength) * 0.025)) *
                    fighter1.Damage);
            }

            if (fighter2.Strength >= fighter1.Armor)
            {
                fighter2Damage = (uint)(((fighter2.Strength - fighter1.Armor) * 0.05 + 1.00) *
                    fighter2.Damage);
            }
            else
            {
                fighter2Damage = (uint)((1.00 - ((fighter1.Armor - fighter2.Strength) * 0.025)) *
                    fighter2.Damage);
            }
        }

        private static void CleanTheMess(Character defeated, World world)
        {
            defeated.presentLocation.Stack.AddRange(defeated.Inventory);
            defeated.Inventory.Clear();
            Garbage.Add(defeated);
        }
    }
}
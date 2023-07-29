using System; // Must include if you are willing to use variables and other materials from the system.
using Microsoft.Xna.Framework; // Must Include for location, positioning, movement, and coloring. It is impossible to make custom AI without this
using Terraria; // Used for general Terraria variables like Player player and NPC npc. Must include if you are willing to go past the point of set defaults and set static defaults.
using Terraria.ID; // Gets all IDs from Terraria.
using Terraria.Localization; // Localization!
using Terraria.Utilities;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ModLoader; // Self Explanatory
using Terraria.ModLoader.Utilities; // Self Explanatoryusing Terraria.ModLoader; // Self Explanatory
using static Terraria.ModLoader.ModContent; // Not sure what it is, but once I find out, I'll update this.

namespace VolpescuBoscaiolo.NPCs
{
    [AutoloadHead]  // Loads the NPC's head on the player map.
    public class Volpescu : ModNPC // Required generic identifier.
    {
        private static bool baseShop = false;
        private static bool tokenShop = false;

        public override string Texture
        {
            get
            {
                return "VolpescuBoscaiolo/NPCs/Volpescu";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mio Padre");

            Main.npcFrameCount[NPC.type] = 25;

            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad,AffectionLevel.Love);
        }

        public override void SetDefaults()
        {
            NPC.friendly = true; // Check if the NPC should be unable to harm the player or any other friendly NPCs.
            NPC.townNPC = true; // Check if the NPC is a town NPC and requires a suitable housing.
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 8;
            NPC.defense = 15;
            NPC.lifeMax = 2500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */ {
                if (NPC.downedSlimeKing && Main.dayTime)
                {
                    return true;
                }
                return false;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20000; // The amount of damage the Town NPC inflicts.
            knockback = 10f; // The amount of knockback the Town NPC inflicts.
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 1; // The amount of ticks the Town NPC takes to cool down. Every 60 in-game ticks is a second.
            randExtraCooldown = 30; // How long it takes until the NPC attacks again, but with a chance.
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = 206; // The Projectile this NPC shoots. Search up Terraria Projectile IDs, I cannot link the websites in this code
            attackDelay = 1; // Delays the attacks, obviously.
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f; // The Speed of the Projectile
            randomOffset = 2f; // Random Offset
        }

        public override string GetChat()
        {
            // these lines of code are an example of how NPCs can refer to others, most NPCs often use this to hint a relationship between them and another.
            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad >= 0 && Main.rand.NextBool(8))
            {
                return "Ma quanto è fregna " + Main.npc[dryad].GivenName + ". Vorrei leccare i suoi piedini sudati.";
            }
            int tavernkeep = NPC.FindFirstNPC(550);
            if (tavernkeep >= 0 && Main.rand.NextBool(8))
            {
                return "Mio padre " + Main.npc[tavernkeep].GivenName + " è un cazzo di ladro.";
            }
            // Generic TownNPC dialogue
            switch (Main.rand.Next(6))
            {
                case 0:
                    return "DE PEFFO'";
                case 1:
                    return "PIE FO'";
                case 2:
                    return "CAPIAMO";
                case 3:
                    return "VOGLIO LECCARE I PIEDINI DI QUALCHE DONNINA ANIME";
                case 4:
                    return "AMO DARIO MOCCIA";
                default:
                    return "PIEFFO PIEFFO PIEFFO PIEFFO PIEFFO PIEFFO";
            }
            var dialog = new WeightedRandom<string>();
            dialog.Add("SI FA A LETTO?");
            return dialog.Get();
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string> { "Volpescu" };
        }

        
		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "Token" + Language.GetTextValue("LegacyInterface.28");
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Shop";
            }
            else
            {
                shopName = "Token Shop";
            }
        }

        public override void AddShops()
        {
            var baseShop = new NPCShop(Type, "Shop");

            baseShop.Add(new Item(ModContent.ItemType<Potions.DeathpointPotion>())
            {
                shopCustomPrice = Item.sellPrice(gold: 3, silver: 54)
            });

            if(NPC.downedBoss2)
            {
                baseShop.Add(new Item(ItemID.LifeCrystal)
                {
                    shopCustomPrice = Item.sellPrice(gold: 10)
                });
            }

            if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                baseShop.Add(new Item(ItemID.LifeFruit)
                {
                    shopCustomPrice = Item.sellPrice(gold: 15)
                });
            }

            var tokenShop = new NPCShop(Type, "Token Shop");

            ModLoader.TryGetMod("Tokens", out Mod Tokens);

            if(Tokens != null)
            {
                if(Tokens.TryFind<ModItem>("PreEyeSurfaceForestToken", out ModItem PreEyeSurfaceForestToken))
                {
                    tokenShop.Add(new Item(PreEyeSurfaceForestToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if(Tokens.TryFind<ModItem>("PreBossLootToken", out ModItem PreBossLootToken))
                {
                    tokenShop.Add(new Item(PreBossLootToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if(Tokens.TryFind<ModItem>("CorruptionTokens", out ModItem CorruptionTokens))
                {
                    tokenShop.Add(new Item(CorruptionTokens.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if(Tokens.TryFind<ModItem>("CrimsonToken", out ModItem CrimsonToken))
                {
                    tokenShop.Add(new Item(CrimsonToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("DesertToken", out ModItem DesertToken))
                {
                    tokenShop.Add(new Item(DesertToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("SnowToken", out ModItem SnowToken))
                {
                    tokenShop.Add(new Item(SnowToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("DungeonToken", out ModItem DungeonToken))
                {
                    tokenShop.Add(new Item(DungeonToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("HellToken", out ModItem HellToken))
                {
                    tokenShop.Add(new Item(HellToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("JungleToken", out ModItem JungleToken))
                {
                    tokenShop.Add(new Item(JungleToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("OceanToken", out ModItem OceanToken))
                {
                    tokenShop.Add(new Item(OceanToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("UndergroundToken", out ModItem UndergroundToken))
                {
                    tokenShop.Add(new Item(UndergroundToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("UndergroundDesertToken", out ModItem UndergroundDesertToken))
                {
                    tokenShop.Add(new Item(UndergroundDesertToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("PrehardmodeFishingToken", out ModItem PrehardmodeFishingToken))
                {
                    tokenShop.Add(new Item(PrehardmodeFishingToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if (Tokens.TryFind<ModItem>("PreHardmodeSpaceToken", out ModItem PreHardmodeSpaceToken))
                {
                    tokenShop.Add(new Item(PreHardmodeSpaceToken.Type)
                    {
                        shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                    });
                }

                if(NPC.downedGoblins)
                {
                    if (Tokens.TryFind<ModItem>("PostGoblinsLootToken", out ModItem PostGoblinsLootToken))
                    {
                        tokenShop.Add(new Item(PostGoblinsLootToken.Type)
                        {
                            shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                        });
                    }
                }

                if(NPC.downedBoss2)
                {
                    if (Tokens.TryFind<ModItem>("PostEyeSurfaceForestToken", out ModItem PostEyeSurfaceForestToken))
                    {
                        tokenShop.Add(new Item(PostEyeSurfaceForestToken.Type)
                        {
                            shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                        });
                    }
                }

                if(Main.hardMode)
                {
                    if (Tokens.TryFind<ModItem>("HardmodeFishingToken", out ModItem HardmodeFishingToken))
                    {
                        tokenShop.Add(new Item(HardmodeFishingToken.Type)
                        {
                            shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                        });
                    }

                    if (Tokens.TryFind<ModItem>("HardmodeLootToken", out ModItem HardmodeLootToken))
                    {
                        tokenShop.Add(new Item(HardmodeLootToken.Type)
                        {
                            shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                        });
                    }

                    if (Tokens.TryFind<ModItem>("HardmodeSpaceToken", out ModItem HardmodeSpaceToken))
                    {
                        tokenShop.Add(new Item(HardmodeSpaceToken.Type)
                        {
                            shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                        });
                    }

                    if(NPC.downedPirates)
                    {
                        if (Tokens.TryFind<ModItem>("PostPiratesLootToken", out ModItem PostPiratesLootToken))
                        {
                            tokenShop.Add(new Item(PostPiratesLootToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }

                    if(NPC.downedMechBoss1)
                    {
                        if (Tokens.TryFind<ModItem>("PostSkeletronLootToken", out ModItem PostSkeletronLootToken))
                        {
                            tokenShop.Add(new Item(PostSkeletronLootToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }

                    if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss1)
                    {
                        if (Tokens.TryFind<ModItem>("PostMechanicalBossLootToken", out ModItem PostMechanicalBossLootToken))
                        {
                            tokenShop.Add(new Item(PostMechanicalBossLootToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }

                    if(NPC.downedPlantBoss)
                    {
                        if (Tokens.TryFind<ModItem>("PostPlanteraLootToken", out ModItem PostPlanteraLootToken))
                        {
                            tokenShop.Add(new Item(PostPlanteraLootToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }

                    if(NPC.downedGolemBoss)
                    {
                        if (Tokens.TryFind<ModItem>("LihzahrdTempleToken", out ModItem LihzahrdTempleToken))
                        {
                            tokenShop.Add(new Item(LihzahrdTempleToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }

                    if(NPC.downedMartians)
                    {
                        if (Tokens.TryFind<ModItem>("PostMartiansLootToken", out ModItem PostMartiansLootToken))
                        {
                            tokenShop.Add(new Item(PostMartiansLootToken.Type)
                            {
                                shopCustomPrice = Item.sellPrice(silver: 40, copper: 54)
                            });
                        }
                    }
                }
            }

            tokenShop.Register();
            baseShop.Register();
        }
    }
}



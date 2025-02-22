using HarmonyLib;
using System;
using System.IO;
using System.Net.Http;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using System.Collections.Generic;
using System.Linq;

using Palette = BLMBFIODBKL;

namespace TheOtherRoles
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManagerUpdatePatch
    {
        static void resetNameTagsAndColors() {
            Dictionary<byte, PlayerControl> playersById = Helpers.allPlayersById();

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                player.nameText.text = player.PPMOEEPBHJO.PCLLABJCIPC;
                if (PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL && player.PPMOEEPBHJO.FDNMBJOAPFL) {
                    player.nameText.color = Palette.JPCHLLEJNEH;
                } else {
                    player.nameText.color = Color.white;
                }
            }
            if (MeetingHud.Instance != null) {
                foreach (PlayerVoteArea player in MeetingHud.Instance.GBKFCOAKLAH) {
                    PlayerControl playerControl = playersById.ContainsKey((byte)player.GEIOMAPOPKA) ? playersById[(byte)player.GEIOMAPOPKA] : null;
                    if (playerControl != null) {
                        player.NameText.text = playerControl.PPMOEEPBHJO.PCLLABJCIPC;
                        if (PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL && playerControl.PPMOEEPBHJO.FDNMBJOAPFL) {
                            player.NameText.color = Palette.JPCHLLEJNEH;
                        } else {
                            player.NameText.color = Color.white;
                        }
                    }
                }
            }
            if (PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL) {
                List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList();
                impostors.RemoveAll(x => !x.PPMOEEPBHJO.FDNMBJOAPFL);
                foreach (PlayerControl player in impostors)
                    player.nameText.color = Palette.JPCHLLEJNEH;
                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.GBKFCOAKLAH) {
                        PlayerControl playerControl = Helpers.playerById((byte)player.GEIOMAPOPKA);
                        if (playerControl != null && playerControl.PPMOEEPBHJO.FDNMBJOAPFL)
                            player.NameText.color =  Palette.JPCHLLEJNEH;
                    }
            }

        }

        static void setPlayerNameColor(PlayerControl p, Color color) {
            p.nameText.color = color;
            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.GBKFCOAKLAH)
                    if (player.NameText != null && p.PlayerId == player.GEIOMAPOPKA)
                        player.NameText.color = color;
        }

        static void setNameColors() {
            if (Jester.jester != null && Jester.jester == PlayerControl.LocalPlayer)
                setPlayerNameColor(Jester.jester, Jester.color);
            else if (Mayor.mayor != null && Mayor.mayor == PlayerControl.LocalPlayer)
                setPlayerNameColor(Mayor.mayor, Mayor.color);
            else if (Engineer.engineer != null && Engineer.engineer == PlayerControl.LocalPlayer)
                setPlayerNameColor(Engineer.engineer, Engineer.color);
            else if (Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Sheriff.sheriff, Sheriff.color);
            else if (Lighter.lighter != null && Lighter.lighter == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Lighter.lighter, Lighter.color);
            else if (Detective.detective != null && Detective.detective == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Detective.detective, Detective.color);
            else if (TimeMaster.timeMaster != null && TimeMaster.timeMaster == PlayerControl.LocalPlayer)
                setPlayerNameColor(TimeMaster.timeMaster, TimeMaster.color);
            else if (Medic.medic != null && Medic.medic == PlayerControl.LocalPlayer)
                setPlayerNameColor(Medic.medic, Medic.color);
            else if (Shifter.shifter != null && Shifter.shifter == PlayerControl.LocalPlayer)
                setPlayerNameColor(Shifter.shifter, Shifter.color);
            else if (Swapper.swapper != null && Swapper.swapper == PlayerControl.LocalPlayer)
                setPlayerNameColor(Swapper.swapper, Swapper.color);
            else if (Lovers.lover1 != null && Lovers.lover2 != null && (Lovers.lover1 == PlayerControl.LocalPlayer || Lovers.lover2 == PlayerControl.LocalPlayer)) {             
                setPlayerNameColor(Lovers.lover1, Lovers.color);
                setPlayerNameColor(Lovers.lover2, Lovers.color);
            }
            else if (Seer.seer != null && Seer.seer == PlayerControl.LocalPlayer)
                setPlayerNameColor(Seer.seer, Seer.color);  
            else if (Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Hacker.hacker, Hacker.color);
            else if (Tracker.tracker != null && Tracker.tracker == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Tracker.tracker, Tracker.color);
            else if (Snitch.snitch != null && Snitch.snitch == PlayerControl.LocalPlayer) 
                setPlayerNameColor(Snitch.snitch, Snitch.color);
            else if (Jackal.jackal != null && Jackal.jackal == PlayerControl.LocalPlayer) {
                // Jackal can see his sidekick
                setPlayerNameColor(Jackal.jackal, Jackal.color);
                if (Sidekick.sidekick != null) {
                    setPlayerNameColor(Sidekick.sidekick, Jackal.color);
                }
                if (Jackal.fakeSidekick != null) {
                    setPlayerNameColor(Jackal.fakeSidekick, Jackal.color);
                }
            }
            else if (Spy.spy != null && Spy.spy == PlayerControl.LocalPlayer) {
                setPlayerNameColor(Spy.spy, Spy.color);
            }
            
            // No else if here, as a Lover of team Jackal needs the colors
            if (Sidekick.sidekick != null && Sidekick.sidekick == PlayerControl.LocalPlayer) {
                // Sidekick can see the jackal
                setPlayerNameColor(Sidekick.sidekick, Sidekick.color);
                if (Jackal.jackal != null) {
                    setPlayerNameColor(Jackal.jackal, Jackal.color);
                }
            }

            // No else if here, as the Impostors need the Spy name to be colored
            if (Spy.spy != null && PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL) {
                setPlayerNameColor(Spy.spy, Spy.color);
            }

            // Crewmate roles with no changes: Child
            // Impostor roles with no changes: Morphling, Camouflager, Vampire, Godfather, Eraser, Janitor and Mafioso
        }

        static void setMafiaNameTags() {
            if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL) {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    if (Godfather.godfather != null && Godfather.godfather == player)
                            player.nameText.text = player.PPMOEEPBHJO.PCLLABJCIPC + " (G)";
                    else if (Mafioso.mafioso != null && Mafioso.mafioso == player)
                            player.nameText.text = player.PPMOEEPBHJO.PCLLABJCIPC + " (M)";
                    else if (Janitor.janitor != null && Janitor.janitor == player)
                            player.nameText.text = player.PPMOEEPBHJO.PCLLABJCIPC + " (J)";
                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.GBKFCOAKLAH)
                        if (Godfather.godfather != null && Godfather.godfather.PlayerId == player.GEIOMAPOPKA)
                            player.NameText.text = Godfather.godfather.PPMOEEPBHJO.PCLLABJCIPC + " (G)";
                        else if (Mafioso.mafioso != null && Mafioso.mafioso.PlayerId == player.GEIOMAPOPKA)
                            player.NameText.text = Mafioso.mafioso.PPMOEEPBHJO.PCLLABJCIPC + " (M)";
                        else if (Janitor.janitor != null && Janitor.janitor.PlayerId == player.GEIOMAPOPKA)
                            player.NameText.text = Janitor.janitor.PPMOEEPBHJO.PCLLABJCIPC + " (J)";
            }
        }

        static void updateShielded() {
            if (Medic.shielded == null) return;

            if(Medic.showShielded == 0) // Everyone
            {
                Medic.shielded.KJAENOGGEOK.material.SetFloat("_Outline",  1f);
                Medic.shielded.KJAENOGGEOK.material.SetColor("_OutlineColor", Medic.shieldedColor);
            }
            else if (Medic.showShielded == 1 && PlayerControl.LocalPlayer == Medic.shielded) // Shielded + Medic
            {
                Medic.shielded.KJAENOGGEOK.material.SetFloat("_Outline", 1f);
                Medic.shielded.KJAENOGGEOK.material.SetColor("_OutlineColor", Medic.shieldedColor);
            }
            else if(PlayerControl.LocalPlayer == Medic.medic) // Medic
            {
                Medic.shielded.KJAENOGGEOK.material.SetFloat("_Outline",  1f);
                Medic.shielded.KJAENOGGEOK.material.SetColor("_OutlineColor", Medic.shieldedColor);
            }

            // Break shield
            if (Medic.shielded.PPMOEEPBHJO.IAGJEKLJCCI || Medic.medic == null || Medic.medic.PPMOEEPBHJO.IAGJEKLJCCI) {
                Medic.shielded.KJAENOGGEOK.material.SetFloat("_Outline", 0f);
                Medic.shielded = null;
            }
        }

        static void timerUpdate() {
            Hacker.hackerTimer -= Time.deltaTime;
            Lighter.lighterTimer -= Time.deltaTime;
            Trickster.lightsOutTimer -= Time.deltaTime;
        }

        static void camouflageAndMorphActions() {
            float oldCamouflageTimer = Camouflager.camouflageTimer;
            float oldMorphTimer = Morphling.morphTimer;

            Camouflager.camouflageTimer -= Time.deltaTime;
            Morphling.morphTimer -= Time.deltaTime;

            // Morphling player size not done here

            // Set morphling morphed look
            if (Morphling.morphTimer > 0f && Camouflager.camouflageTimer <= 0f) {
                if (Morphling.morphling != null && Morphling.morphTarget != null) {
                    Morphling.morphling.nameText.text = Morphling.morphTarget.PPMOEEPBHJO.PCLLABJCIPC;
                    Morphling.morphling.KJAENOGGEOK.material.SetColor("_BackColor", Palette.PHFOPNDOEMD[Morphling.morphTarget.PPMOEEPBHJO.IMMNCAGJJJC]);
                    Morphling.morphling.KJAENOGGEOK.material.SetColor("_BodyColor", Palette.AEDCMKGJKAG[Morphling.morphTarget.PPMOEEPBHJO.IMMNCAGJJJC]);
                    Morphling.morphling.KJAENOGGEOK.material.SetFloat("_Outline",  Morphling.morphTarget.KJAENOGGEOK.material.GetFloat("_Outline"));
                    Morphling.morphling.KJAENOGGEOK.material.SetColor("_OutlineColor", Morphling.morphTarget.KJAENOGGEOK.material.GetColor("_OutlineColor"));
                    Morphling.morphling.HatRenderer.SetHat(Morphling.morphTarget.PPMOEEPBHJO.CPGFLBANALE, Morphling.morphTarget.PPMOEEPBHJO.IMMNCAGJJJC);
                    Morphling.morphling.nameText.transform.localPosition = new Vector3(0f, (Morphling.morphTarget.PPMOEEPBHJO.CPGFLBANALE == 0U) ? 0.7f : 1.05f, -0.5f);

                    if (Morphling.morphling.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.CHNDKKBEIDG.AllSkins[(int)Morphling.morphTarget.PPMOEEPBHJO.CGNMKICGLOG].ProdId) {
                        Helpers.setSkinWithAnim(Morphling.morphling.MyPhysics, Morphling.morphTarget.PPMOEEPBHJO.CGNMKICGLOG);
                    }
                    if (Morphling.morphling.CurrentPet == null || Morphling.morphling.CurrentPet.OPADMIKFGHK != DestroyableSingleton<HatManager>.CHNDKKBEIDG.AllPets[(int)Morphling.morphTarget.PPMOEEPBHJO.LBHODBKCBKA].OPADMIKFGHK) {
                        if (Morphling.morphling.CurrentPet) UnityEngine.Object.Destroy(Morphling.morphling.CurrentPet.gameObject);
                        Morphling.morphling.CurrentPet = UnityEngine.Object.Instantiate<PetBehaviour>(DestroyableSingleton<HatManager>.CHNDKKBEIDG.AllPets[(int)Morphling.morphTarget.PPMOEEPBHJO.LBHODBKCBKA]);
                        Morphling.morphling.CurrentPet.transform.position = Morphling.morphling.transform.position;
                        Morphling.morphling.CurrentPet.Source = Morphling.morphling;
                        Morphling.morphling.CurrentPet.BDBDGFDELMB = Morphling.morphling.BDBDGFDELMB;
                        PlayerControl.SetPlayerMaterialColors(Morphling.morphTarget.PPMOEEPBHJO.IMMNCAGJJJC, Morphling.morphling.CurrentPet.rend);
                    } else if (Morphling.morphling.CurrentPet) {
                        PlayerControl.SetPlayerMaterialColors(Morphling.morphTarget.PPMOEEPBHJO.IMMNCAGJJJC, Morphling.morphling.CurrentPet.rend);
                    }
                }
            }

            // Set camouflaged look (overrides morphling morphed look if existent)
            if (Camouflager.camouflageTimer > 0f) {
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    p.nameText.text = "";
                    p.KJAENOGGEOK.material.SetColor("_BackColor", Palette.AEDCMKGJKAG[6]);
                    p.KJAENOGGEOK.material.SetColor("_BodyColor", Palette.AEDCMKGJKAG[6]);
                    p.KJAENOGGEOK.material.SetFloat("_Outline",  0f);
                    p.HatRenderer.SetHat(0, 0);
                    Helpers.setSkinWithAnim(p.MyPhysics, 0);
                    bool spawnPet = false;
                    if (p.CurrentPet == null) spawnPet = true;
                    else if (p.CurrentPet.OPADMIKFGHK != DestroyableSingleton<HatManager>.CHNDKKBEIDG.AllPets[0].OPADMIKFGHK) {
                        UnityEngine.Object.Destroy(p.CurrentPet.gameObject);
                        spawnPet = true;
                    }

                    if (spawnPet) {
                        p.CurrentPet = UnityEngine.Object.Instantiate<PetBehaviour>(DestroyableSingleton<HatManager>.CHNDKKBEIDG.AllPets[0]);
                        p.CurrentPet.transform.position = p.transform.position;
                        p.CurrentPet.Source = p;
                    }
                }
            } 
            
            // Everyone but morphling reset
            if (oldCamouflageTimer > 0f && Camouflager.camouflageTimer <= 0f) {
                Camouflager.resetCamouflage();
            }

            // Morphling reset
            if ((oldMorphTimer > 0f || oldCamouflageTimer > 0f) && Camouflager.camouflageTimer <= 0f && Morphling.morphTimer <= 0f && Morphling.morphling != null) {
                Morphling.resetMorph();
            }
        }

        public static void childUpdate() {
            if (Child.child == null || Camouflager.camouflageTimer > 0f) return;
                
            float growingProgress = Child.growingProgress();
            float scale = growingProgress * 0.35f + 0.35f;
            string suffix = "";
            if (growingProgress != 1f)
                suffix = " <color=#FAD934FF>(" + Mathf.FloorToInt(growingProgress * 18) + ")</color>"; 

            Child.child.nameText.text += suffix;
            if (MeetingHud.Instance != null) {
                foreach (PlayerVoteArea player in MeetingHud.Instance.GBKFCOAKLAH)
                    if (player.NameText != null && Child.child.PlayerId == player.GEIOMAPOPKA)
                        player.NameText.text += suffix;
            }

            if (Morphling.morphling != null && Morphling.morphTarget == Child.child && Morphling.morphTimer > 0f)
                Morphling.morphling.nameText.text += suffix;
        }

        static void updateImpostorKillButton(HudManager __instance) {
            if (!PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL) return;
            bool enabled = true;
            if (Vampire.vampire != null && Vampire.vampire == PlayerControl.LocalPlayer)
                enabled = false;
            else if (Mafioso.mafioso != null && Mafioso.mafioso == PlayerControl.LocalPlayer && Godfather.godfather != null && !Godfather.godfather.PPMOEEPBHJO.IAGJEKLJCCI)
                enabled = false;
            else if (Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer)
                enabled = false;
            enabled &= __instance.UseButton.isActiveAndEnabled;
            
            __instance.KillButton.gameObject.SetActive(enabled);
            __instance.KillButton.renderer.enabled = enabled;
            __instance.KillButton.isActive = enabled;
            __instance.KillButton.enabled = enabled;
        }

        static void snitchUpdate() {
            if (Snitch.localArrows == null) return;

            foreach (Arrow arrow in Snitch.localArrows) arrow.arrow.SetActive(false);

            if (Snitch.snitch == null || Snitch.snitch.PPMOEEPBHJO.IAGJEKLJCCI) return;

            int numberOfTasks = 0;
            GameData.LGBOMGHJELL LGBOMGHJELL = Snitch.snitch.PPMOEEPBHJO;
			if (!LGBOMGHJELL.MFFAGDHDHLO && LGBOMGHJELL.PHGPJMKOKMC != null) {
				for (int i = 0; i < LGBOMGHJELL.PHGPJMKOKMC.Count; i++) {
					if (!LGBOMGHJELL.PHGPJMKOKMC[i].LBBFBHJINJK)
						numberOfTasks++;
				}
			}

            if (PlayerControl.LocalPlayer.PPMOEEPBHJO.FDNMBJOAPFL && numberOfTasks <= Snitch.taskCountForImpostors) {
                if (Snitch.localArrows.Count == 0) Snitch.localArrows.Add(new Arrow(Color.blue));
                if (Snitch.localArrows.Count != 0 && Snitch.localArrows[0] != null) {
                    Snitch.localArrows[0].arrow.SetActive(true);
                    Snitch.localArrows[0].Update(Snitch.snitch.transform.position);
                }
            } else if (PlayerControl.LocalPlayer == Snitch.snitch && numberOfTasks == 0) { 
                int arrowIndex = 0;
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    if (p.PPMOEEPBHJO.FDNMBJOAPFL && !p.PPMOEEPBHJO.IAGJEKLJCCI) {
                        if (arrowIndex >= Snitch.localArrows.Count) Snitch.localArrows.Add(new Arrow(Color.blue));
                        if (arrowIndex < Snitch.localArrows.Count && Snitch.localArrows[arrowIndex] != null) {
                            Snitch.localArrows[arrowIndex].arrow.SetActive(true);
                            Snitch.localArrows[arrowIndex].Update(p.transform.position);
                        }
                        arrowIndex++;
                    }
                }
            }
        }

        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GCDONLGCMIL.Started) return;

            CustomButton.HudUpdate();
            resetNameTagsAndColors();
            setNameColors();
            updateShielded();

            // Mafia
            setMafiaNameTags();
            // Jester
            Helpers.removeTasksFromPlayer(Jester.jester);
            // Impostors
            updateImpostorKillButton(__instance);
            // Timer updates
            timerUpdate();
            // Camouflager and Morphling
            camouflageAndMorphActions();
            // Child
            childUpdate();
            // Snitch
            snitchUpdate();
            // Jackal
            Helpers.removeTasksFromPlayer(Jackal.jackal);
            // Sidekick
            Helpers.removeTasksFromPlayer(Sidekick.sidekick);
        }
    }
}
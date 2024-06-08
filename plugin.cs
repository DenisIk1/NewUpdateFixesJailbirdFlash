﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem;
using System;
using System.Collections.Generic;

namespace NewUpdateFixes
{
    public class ColaFix : Plugin<Config>
    {
        internal GenHandler GenHandler { get; private set; }

        public override string Name => "NewUpdateFixes";

        public override string Prefix => "NUF";

        public override string Author => "Half";

        public override Version Version => new Version(1, 1, 0);

        public override Version RequiredExiledVersion => new Version(8, 9, 2);

        

        public override void OnEnabled()
        {

            base.OnEnabled();

            SubscribeEvents();
        }

        public override void OnDisabled()
        {

            UnsubscribeEvents();

            base.OnDisabled();
        }

        protected void SubscribeEvents()
        {

            GenHandler = new(Config);

            Exiled.Events.Handlers.Player.UsingItem += GenHandler.OnUsingItem;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += GenHandler.OnUpgradingInventoryItem;
            Exiled.Events.Handlers.Scp914.UpgradingPickup += GenHandler.OnUpgradingPickup;
            Exiled.Events.Handlers.Player.Hurting += GenHandler.OnHurting;
            
        }
        protected void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingItem -= GenHandler.OnUsingItem;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= GenHandler.OnUpgradingInventoryItem;
            Exiled.Events.Handlers.Scp914.UpgradingPickup -= GenHandler.OnUpgradingPickup;
            Exiled.Events.Handlers.Player.Hurting -= GenHandler.OnHurting;

            GenHandler = null;
        }
    }


    internal class GenHandler
    {
        private Config config;

        private Dictionary<float, float> colaDamage1 = new()
        {
            { 0.15f, 0.1f },
            { 0.225f, 0.15f},
            { 0.9f, 0.4f},
            { 1.5f, 1f},
        };
        private Dictionary<float, float> colaDamage2 = new()
        {
            { 0.25f, 0.15f},
            { 0.375f, 0.23f},
            { 1.5f, 0.6f},
            { 2.5f, 1.5f},
        };
        private Dictionary<float, float> colaDamage3 = new()
        {
            { 0.4f, 0.25f },
            { 0.6f, 0.38f} ,
            { 2.4f, 1f},
            { 4f, 2.5f}
        };

        internal GenHandler(Config instance)
        {
            config = instance;
        }
        internal void UpgradeItemFloor(ItemType input, ItemType result, Scp914.Scp914KnobSetting knobSetting, int chance, UpgradingPickupEventArgs ev)
        {
            if (ev.KnobSetting == knobSetting && ev.Pickup.Type == input)
            {
                var random = UnityEngine.Random.Range(1, chance);
                if (random == 1)
                {
                    Exiled.API.Features.Pickups.Pickup.CreateAndSpawn(result, ev.OutputPosition, ev.Pickup.Rotation);
                }

                ev.Pickup.Destroy();
            }

        }

        internal void UpgradeItemHand(ItemType input, ItemType result, Scp914.Scp914KnobSetting knobSetting, int chance, UpgradingInventoryItemEventArgs ev)
        {
            if (ev.KnobSetting == knobSetting && ev.Item.Type == input)
            {
                ev.Player.CurrentItem.Destroy();
                var random = UnityEngine.Random.Range(1, chance);
                if (random == 1)
                {
                    ev.Player.Inventory.ServerAddItem(result);
                }
            }

        }

        internal void OnHurting(HurtingEventArgs ev)
        {
            if (config.OldColaHealthDrain)
            {
                if (ev.DamageHandler.Type == DamageType.Scp207)
                {
                    byte intensity = ev.Player.GetEffect(EffectType.Scp207).Intensity;
                    

                    if (intensity == 1)
                    {
                        float newDamage;
                        colaDamage1.TryGetValue(ev.Amount, out newDamage);
                        ev.Amount = newDamage;
                    }
                    if (intensity == 2)
                    {
                        float newDamage;
                        colaDamage2.TryGetValue(ev.Amount, out newDamage);
                        ev.Amount = newDamage;
                    }
                    if (intensity == 3)
                    {
                        float newDamage;
                        colaDamage3.TryGetValue(ev.Amount, out newDamage);
                        ev.Amount = newDamage;
                    }
                }
            }
            
        }

        internal void OnUpgradingPickup(UpgradingPickupEventArgs ev)
        {
            if (config.OldColaRecipes914Dropped)
            {
                UpgradeItemFloor(ItemType.Adrenaline, ItemType.SCP1853, Scp914.Scp914KnobSetting.VeryFine, 3, ev);

            }
        }

        internal void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
        {
            if (config.OldColaRecipes914Hand)
            {
                
                UpgradeItemHand(ItemType.Adrenaline, ItemType.SCP1853, Scp914.Scp914KnobSetting.VeryFine, 3, ev);

            }
        }

        internal void OnUsingItem(UsingItemEventArgs ev)
        {
            
            

            if (config.OldColaSpeed)
            {
                byte intensity = ev.Player.GetEffect(EffectType.Scp207).Intensity;

                if (ev.Item.Type == ItemType.SCP207)
                {
                    
                    if (intensity == 0)
                    {

                        ev.Player.EnableEffect(EffectType.MovementBoost, 9);
                    }
                    else if (intensity == 1)
                    {
                        ev.Player.DisableEffect(EffectType.MovementBoost);
                        ev.Player.EnableEffect(EffectType.MovementBoost, 15);
                    }
                    else if (intensity == 2)
                    {
                        ev.Player.DisableEffect(EffectType.MovementBoost);
                        ev.Player.EnableEffect(EffectType.MovementBoost, 7);
                    }

                }
            }


            if (ev.Item.Type == ItemType.SCP500)
            {
                if (config.Scp500CuresTrauma)
                {
                    ev.Player.DisableEffect(EffectType.Traumatized);
                }

                if (config.OldColaSpeed) 
                {
                    ev.Player.DisableEffect(EffectType.MovementBoost);
                }
            }
        }
    }
}

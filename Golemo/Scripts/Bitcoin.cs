using GTANetworkAPI;
using System.Collections.Generic;
using System;
using Golemo.GUI;
using Golemo.Core;
using GolemoSDK;
using Golemo.MoneySystem;

namespace Golemo.Scripts
{
    class Bitcoin : Script
    {
        private static nLog Log = new nLog("Bitcoin");
        private static Random rnd = new Random();

        private static Dictionary<int, ColShape> Colsh = new Dictionary<int, ColShape>();
        private static int BitcoinMultiplier;    //коэффициент
        private static int BitcoinsPayment = 36000;  //статичная цена
        private static int minMultiplier = 2; // минимальный коеффициент
        private static int maxMultiplier = 20; // максимальный коеффициеннтЙ

        private void cfr_onEntityEnterColShape(ColShape shape, Player entity)
        {
            try
            {
                NAPI.Data.SetEntityData(entity, "INTERACTIONCHECK", shape.GetData<int>("INTERACT"));
                UpdateLabelMulti();
            }
            catch (Exception ex) { Log.Write("gp_onEntityEnterColShape: " + ex.Message, nLog.Type.Error); }
        }
        private void cfr_onEntityExitColShape(ColShape shape, Player entity)
        {
            try
            {
                NAPI.Data.SetEntityData(entity, "INTERACTIONCHECK", 9134);
            }
            catch (Exception ex) { Log.Write("gp_onEntityExitColShape: " + ex.Message, nLog.Type.Error); }
        }
        public TextLabel label = null;
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart() //создаем colshape
        {
            try
            {
                Colsh.Add(4, NAPI.ColShape.CreateCylinderColShape(new Vector3(-557.05615, -187.82167, 37.22109), 1, 2, 0)); // get clothes
                Colsh[4].OnEntityEnterColShape += cfr_onEntityEnterColShape;
                Colsh[4].OnEntityExitColShape += cfr_onEntityExitColShape;
                Colsh[4].SetData("INTERACT", 9134);
                NAPI.TextLabel.CreateTextLabel(Main.StringToU16("~w~Нажмите Е"), new Vector3(-557.05615, -187.82167, 40.22109) + new Vector3(0, 0, 1), 10F, 0.6F, 0, new Color(0, 180, 0));
                label = NAPI.TextLabel.CreateTextLabel(Main.StringToU16($"~w~Курс еще не известен."), new Vector3(-557.05615, -187.82167, 38.22109) + new Vector3(0, 0, 1), 10F, 0.6F, 0, new Color(0, 180, 0));
                NAPI.TextLabel.CreateTextLabel(Main.StringToU16("~r~Информатор Крипто"), new Vector3(-557.05615, -187.82167, 37.22109) + new Vector3(0, 0, 1), 10F, 0.6F, 0, new Color(0, 180, 0));
                UpdateMultiplierInConsole();
                UpdateLabelMulti();

            }
            catch (Exception e) { Log.Write("ResourceStart: " + e.Message, nLog.Type.Error); }
        }

        public void UpdateLabelMulti() //обновляем ценник в label что бы отображалась цена за биток
        {
            string text = $"~w~Курс {BitcoinsPayment * BitcoinMultiplier} за 1 Bitcoin"; // если надо, тут меняем цену в TextLable
            label.Text = Main.StringToU16(text);

        }
        public static void UpdateMultiplierInConsole() //Обновляем ценник для консольки
        {
            BitcoinMultiplier = rnd.Next(minMultiplier, maxMultiplier);
            Log.Write($"КФ За Биткоин: {BitcoinMultiplier }");
        }
        public static void OpenCryptoPed(Player player) // Подключение менюшки
        {
            Trigger.ClientEvent(player, "NPC.cameraOn" ,1500);
            Trigger.ClientEvent(player, "OpenBuyBitcoin");
        }
          #region Покупка Крипты
        [RemoteEvent("BuyBitcoin:Server")] //Ивент для подключения
        public static void BuyBitcoin_Server(Player player, int id)
        {
            try
            {
                if (!Main.Players.ContainsKey(player)) return;
                int payment = Convert.ToInt32((BitcoinsPayment * BitcoinMultiplier)); // количество * fix-price * коеффициент
                if (Main.Players[player].Money < payment)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно средств", 3000);
                    return;
                }
                MoneySystem.Wallet.Change(player, -payment);
                MoneySystem.Wallet.ChangeCrypto(player, 1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы купили 1 биткоин  за {payment}$", 3000);
            }
            catch (Exception e) { Log.Write("BuyBitcoin_Server: " + e.Message, nLog.Type.Error); }
        }
        #endregion
        #region Продажа Крипты
        [RemoteEvent("SellBitcoin:Server")] //Ивент для подключения
        public static void SellBitcoin_Server(Player player, int id) 
        {
            try
            {
                if (Main.Players[player].Crypto < 1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно биткойнов", 3000);
                    return;
                }
                if (!Main.Players.ContainsKey(player)) return;
                int payment = Convert.ToInt32((BitcoinsPayment * BitcoinMultiplier)); // количество * fix-price * коеффициент
                MoneySystem.Wallet.Change(player, payment);
                MoneySystem.Wallet.ChangeCrypto(player, -1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали биткойн за {payment}$", 3000);
            }
            catch (Exception e) { Log.Write("BuyBitcoin_Server: " + e.Message, nLog.Type.Error); }
        }
        #endregion
         #region Покупка Лакивилспинов
        [RemoteEvent("BuyLuckyWheelBIT:Server")] //Ивент для подключения
        public static void BuyLuckyWheelBIT_Server(Player player, int id)
        {
            try
            {
                if (Main.Players[player].Crypto < 5)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно биткойнов", 3000);
                    return;
                }
                if (!Main.Players.ContainsKey(player)) return;
                MoneySystem.Wallet.ChangeCrypto(player, -5);
                 MoneySystem.Wallet.ChangeLuckyWheelSpins(player, 1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы приобрели 1 прокрут за 1 Биткойн", 3000);
            }
            catch (Exception e) { Log.Write("BuyBitcoin_Server: " + e.Message, nLog.Type.Error); }
        }
        #endregion
    }
    }
        

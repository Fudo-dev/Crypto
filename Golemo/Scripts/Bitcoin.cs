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

        private static Dictionary<int, ColShape> Cols = new Dictionary<int, ColShape>();
        private static int BitcoinMultiplier;    //коэффициент
        private static int BitcoinsPayment = 36000;  //статичная цена
        private static int minMultiplier = 2; // минимальный коеффициент
        private static int maxMultiplier = 20; // максимальный коеффициеннтЙ

        public TextLabel label = null;
        [ServerEvent(Event.ResourceStart)]

        public static void UpdateMultiplierInConsole() //Обновляем ценник для консольки
        {
            BitcoinMultiplier = rnd.Next(minMultiplier, maxMultiplier);
            Log.Write($"КФ За Биткоин: {BitcoinMultiplier }");
        }
        #region Покупка Крипты
        [RemoteEvent("BuyBitcoin:Server")] //Ивент для подключения
        public static void BuyBitcoin_Server(Player player, int id)
        {
            try
            {
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
                int payment = Convert.ToInt32((BitcoinsPayment * BitcoinMultiplier)); // количество * fix-price * коеффициент
                MoneySystem.Wallet.Change(player, payment);
                MoneySystem.Wallet.ChangeCrypto(player, -1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали биткойн за {payment}$", 3000);
            }
            catch (Exception e) { Log.Write("BuyBitcoin_Server: " + e.Message, nLog.Type.Error); }
        }
        #endregion
        #region Покупка Лакивилспинов
        [RemoteEvent("BuyLuckyWheelBIT:Server")] //Ивент для подключени
        public static void BuyLuckyWheelBIT_Server(Player player, int id)
        {
            try
            {
                if (Main.Players[player].Crypto < 5)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно биткойнов", 3000);
                    return;
                }
                MoneySystem.Wallet.ChangeCrypto(player, -5);
                MoneySystem.Wallet.ChangeLuckyWheelSpins(player, 1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы приобрели 1 прокрут за 1 Биткойн", 3000);
            }
            catch (Exception e) { Log.Write("BuyBitcoin_Server: " + e.Message, nLog.Type.Error); }
        }
        #endregion
    }
}


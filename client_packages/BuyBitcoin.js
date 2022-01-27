let Bitcoin;
mp.events.add('OpenBuyBitcoin', () => {
	if (global.menuCheck() || Bitcoin != null) return;
    global.menuOpen();
	Bitcoin = mp.browsers.new('package://browser/modules/Crypto/BuyBitcoin/index.html');
});
mp.events.add('BuyBitcoin', (id) => {
	mp.events.call("CloseBuy");
	mp.events.callRemote("BuyBitcoin:Server", id);
});
mp.events.add('SellBitcoin', (id) => {
	mp.events.call("CloseBuy");
	mp.events.callRemote("SellBitcoin:Server", id);
});
mp.events.add('BuyLuckyWheel', (id) => {
	mp.events.call("CloseBuy");
	mp.events.callRemote("BuyLuckyWheelBIT:Server", id);
});
mp.events.add('CloseBuy', () => {
	if (Bitcoin == null) return;
	Bitcoin.destroy();
	Bitcoin = null;
	global.menuClose();
	mp.events.call("NPC.cameraOff", 1500);
});
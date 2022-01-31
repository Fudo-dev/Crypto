let Bitcoin;
BitcoinM = mp.browsers.new('http://package/browser/modules/Crypto/BuyBitcoin/index.html');
BitcoinN = false;
mp.keys.bind(Keys.VK_O, false, function () { // F6 key report menu
	if (!loggedin || chatActive || editing || cuffed || localplayer.isInAnyVehicle(true) || global.menuOpened || localplayer.getVariable('InDeath') == true || global.IsFalling) return;
    if (!global.menuOpened) {
        global.menuOpen();
        mp.gui.cursor.visible = true;
        BitcoinN = true;
        BitcoinM.execute('CryptoMenu.active=true;');
    } else {
        BitcoinM.execute('CryptoMenu.active=false;');
        global.menuClose();
        BitcoinN = false;
        mp.gui.cursor.visible = false
    }
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
	if(board == null) return;
    global.menuClose();
	BitcoinM.execute('CryptoMenu.active=false');
    BitcoinN = false;
});
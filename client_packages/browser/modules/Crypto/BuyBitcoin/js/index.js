var GovPedMenu = new Vue({
    el: ".ped",
    data: {
	active: false,
	menu: 0,
	style: 0,
	fracid: 6,
	namep: "Fudo",
	surnamep: "4erepaha",
	price: 100,
	workdaystatus: false,
    },
    methods:{
        gostyle: function(index) {
            this.style = index;
        },
		open: function(id){
            this.menu = id;
        },
		buybit: function(id) {
			mp.trigger("BuyBitcoin", id);
		},
		sellbit: function(id) {
			mp.trigger("SellBitcoin", id);
		},
		buyluckywheel: function(id) {
			mp.trigger("BuyLuckyWheel", id);
		},
    }
});
function closemenu() {
    mp.trigger("CloseBuy")
}
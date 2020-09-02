 // МЕНЮ


var menuBtn = document.querySelector('.menu_btn');
var menu = document.querySelector('.menu_wrapper');


menuBtn.onclick = function headerMenu() {
    if (toggle.menu == true) {
        toggleCheck();
        menu.classList.add('menu-active');
        toggle.menu = false;
    } else {
        menu.classList.remove('menu-active');
        toggle.menu = true;
    } 
}

//    Рычаги

var toggle = {
    menu: true,
};

function toggleCheck() {
    if (toggle.menu == false) {
      menu.classList.remove('menu-active');
      toggle.menu = true;
    }
}
var header = document.querySelector('.page-header_promo');
var main = document.querySelector('.main');
var footer = document.querySelector('.page-footer');
var login = document.querySelector('.header-top_useraccount'); 

main.onclick = function() {
    toggleCheck();
}
header.onclick = function() {
    toggleCheck();
}
login.onclick = function() {
    toggleCheck();
}
footer.onclick = function() {
    toggleCheck();
}
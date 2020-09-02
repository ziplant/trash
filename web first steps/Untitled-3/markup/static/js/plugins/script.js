


var menuBtn = document.querySelector('.header_menu-btn');
var menu = document.querySelector('.header_menu');


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
var main = document.getElementById('main');
var login = document.querySelector('.sign-up'); 

main.onclick = function() {
    toggleCheck();
}
login.onclick = function() {
    toggleCheck();
}

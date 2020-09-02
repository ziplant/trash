"use strict";

var menu = $('.menu');
var menuBtn = $('.menu-btn');
var headerImg = $('.header_img');

var sidebar = $('.sidebar-item_content');
var sidebarBtn = $('.sidebar-item_name');
var open = false;

var sliderItems = $('.slider-item');
var leftArrow = $('.slider_btn-left');
var rightArrow = $('.slider_btn-right');
var resizeBtn = $('.slider_resize-btn');
var sliderMain = $('.photo-big');
var slide = 1;

function closeAll() {
    if (open == true) {
        open = false;
        sidebarBtn.removeClass('sidebar_active');
        menu.removeClass('menu_active');
        menuBtn.removeClass('menu-btn_active');
        menuBtn.addClass('menu-btn_close');
        sidebarBtn.removeClass('sidebar_active');
        $('.sidebar-item_content').removeClass('move');
    } else {
        open = true;
    }
}

$(document).ready(function(){
    menuBtn.click(function() {
        menu.addClass('menu_active');
        menuBtn.addClass('menu-btn_active');
        menuBtn.removeClass('menu-btn_close');
        closeAll();
    });
    sidebarBtn.click(function() {
        var sidebarItem = '.' + this.dataset.sidebar;
        $(this).addClass('sidebar_active');
        $(sidebarItem).addClass('move');
        closeAll();
    });
    sliderItems.click(function() {
        sliderItems.removeClass('active-bg');
        $(this).addClass('active-bg');
        slide = this.dataset.slide;
        sliderMain.html('<img src="img/photo-' + slide + '.png" class="main-photo">');

    });
    rightArrow.click(function() {
        if (slide < sliderItems.length) {
            slide = +slide + 1;
        } else {
            slide = 1
        }
        sliderMain.html('<img src="img/photo-' + slide + '.png" class="main-photo">');
        sliderItems.removeClass('active-bg');
        $('li[data-slide="'+ slide +'"').addClass('active-bg');
    });
    leftArrow.click(function() {
        if (slide <= 1) {
            slide = sliderItems.length;
        } else {
            slide = +slide - 1;
        }
        sliderMain.html('<img src="img/photo-' + slide + '.png" class="main-photo">');
        sliderItems.removeClass('active-bg');
        $('li[data-slide="'+ slide +'"').addClass('active-bg');
    });
});


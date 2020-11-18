function createSlider() {
  var container = document.querySelector(".slider_slides");
  var controls = document.querySelectorAll(".slider_item");
  var rightArrow = document.querySelector(".slider_arrow-right");
  var leftArrow = document.querySelector(".slider_arrow-left");
  var curSlide = 0;
  var pixels = 0;
  var imageWidth = controls[0].offsetWidth;
  var slides = countSlides();

  function countSlides() {
    return (
      controls.length -
      (container.offsetWidth - (container.offsetWidth % imageWidth)) /
        imageWidth
    );
  }
  function lastSlideOffset() {
    return (
      imageWidth -
      (container.offsetWidth -
        (imageWidth *
          (container.offsetWidth - (container.offsetWidth % imageWidth))) /
          imageWidth)
    );
  }

  function nextSlide() {
    if (curSlide < slides) {
      curSlide++;
      if (curSlide == slides) {
        pixels += lastSlideOffset();
      } else {
        pixels += imageWidth;
      }
    } else {
      pixels = 0;
      curSlide = 0;
    }

    var move = pixels + "px";

    for (var i = 0; i < controls.length; i++) {
      controls[i].style.right = move;
    }
  }

  function previousSlide() {
    if (curSlide >= 1) {
      curSlide--;
      if (curSlide == 0 && pixels < imageWidth) {
        pixels -= lastSlideOffset();
      } else {
        pixels -= imageWidth;
      }
    } else {
      pixels = imageWidth * slides - (imageWidth - lastSlideOffset());
      curSlide = slides;
    }
    var move = pixels + "px";

    for (var i = 0; i < controls.length; i++) {
      controls[i].style.right = move;
    }
  }

  return function () {
    rightArrow.onclick = nextSlide;
    leftArrow.onclick = previousSlide;

    rightArrow.onmouseover = function () {
      slides = countSlides();
    };
    leftArrow.onmouseover = function () {
      slides = countSlides();
    };
  };
}

var slider = createSlider();

slider();

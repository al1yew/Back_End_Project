$(document).ready(function () {

    //----------------------------------------------- Blog page Authors slider

    $('.author-slider').slick();


    //----------------------------------------------- Product Modal

    $(".detailmodal").click(function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        console.log(url);

        fetch(url).then(response => {
            return response.text();
        })
            .then(data => {
                $(".modal-content").html(data);

                // prodct details slider active
                $('.product-large-slider').slick({
                    fade: true,
                    arrows: false,
                    asNavFor: '.pro-nav'
                });


                // product details slider nav active
                $('.pro-nav').slick({
                    slidesToShow: 4,
                    asNavFor: '.product-large-slider',
                    arrows: false,
                    focusOnSelect: true
                });


                // quantity change js
                $('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
                $('.pro-qty').append('<span class="inc qtybtn">+</span>');
                $('.qtybtn').on('click', function () {
                    var $button = $(this);
                    var oldValue = $button.parent().find('input').val();
                    if ($button.hasClass('inc')) {
                        var newVal = parseFloat(oldValue) + 1;
                    } else {
                        // Don't allow decrementing below zero
                        if (oldValue > 0) {
                            var newVal = parseFloat(oldValue) - 1;
                        } else {
                            newVal = 0;
                        }
                    }
                    $button.parent().find('input').val(newVal);
                });


            })

    });


    //----------------------------------------------- Product Search

    $(".input-search").keyup(function () {
        let inputvalue = $(this).val();

        let url = $(this).data('url');

        url = url + '?search=' + inputvalue;

        if (inputvalue) {
            console.log(inputvalue)
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $(".search-body .list-group").html(data);
                })
        }
        else {
            $(".search-body .list-group").html('');
        }
    });


    //----------------------------------------------- Blog Search

    $(".blog-search").keyup(function () {
        let inputvalue = $(this).val();

        let url = $(this).data('url');

        url = url + '?blogsearch=' + inputvalue;

        if (inputvalue) {
            console.log(inputvalue)
            $(".search-body-blog").removeClass("d-none")
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $(".search-body-blog .list-group-blog").html(data);
                })
        }
        else {
            $(".search-body-blog .list-group-blog").html('');
            $(".search-body-blog").addClass("d-none");
        }
    });


    //-----------------------------------------------  Search For Blogs in blog detail page 

    $(".search-button-blog").click(function (e) {
        e.preventDefault();

        let inputvalue = $(this).prev().val();

        if (!inputvalue) {
            return;
        }

        let url = $(this).data('url');

        url = url + '?SortBySearch=' + inputvalue;

        console.log(url)
        if (inputvalue) {
            console.log(url)
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $(".blog-col-lg-9").html(data);
                    inputvalue = null;
                    $(".search-body-blog").addClass("d-none");
                })
            window.location.href = url
        }
        else {
            $(".blog-col-lg-9").html('');
        }
    });


    //----------------------------------------------- Main Header Search For products

    $(".search-button-product").click(function (e) {
        e.preventDefault();

        let inputvalue = $(this).prev().val();

        if (!inputvalue) {
            return;
        }

        let url = $(this).data('url');

        url = url + '?SortBySearch=' + inputvalue;

        console.log(window.location.href)

        if (inputvalue) {
            console.log(url)
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $(".product-content-container").html(data);
                })
            window.location.href = url
        }
    });

});

//--------------------------------------WRITTEN IN GLOBAL-----------------------------------

//----------------------------------------------- Get the value of select option

//$('.sortbycountproduct').on('change', function () {
//    console.log($(this).val())
//    let val = $(this).val()
//    $.ajax({
//        type: "POST",
//        url: "/Product/Index",
//        "data": "{sortby: `val` }",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            console.log("salam");
//        }
//    });
//});

//$('.sortbyproduct').on('change', function () {
//    console.log($(this).val())
//});


////----------------------------------------------- pricing filter

//var rangeSlider = $(".price-range"),
//    amount = $("#amount"),
//    minPrice = rangeSlider.data('min'),
//    maxPrice = rangeSlider.data('max');
//rangeSlider.slider({
//    range: true,
//    min: minPrice,
//    max: maxPrice,
//    values: [minPrice, maxPrice],
//    slide: function (event, ui) {
//        amount.val("$" + ui.values[0] + " - $" + ui.values[1]);
//    }
//});
//amount.val(" $" + rangeSlider.slider("values", 0) +
//    " - $" + rangeSlider.slider("values", 1));

//$(".price-filter").click(function (e) {
//    e.preventDefault()
//    $.post("/Product/Index", { amount: $('#amount').val() }, function (data, status) {
//        //Response callback
//        alert("Data: " + data + "\nStatus: " + status);
//        console.log("Data: " + data + "\nStatus: " + status)
//    });
//});



//----------------------------------------------- Sliders From Active.js

$(document).ready(function () {

    // Hero main slider active
    $('.hero-slider-active').slick({
        fade: true,
        autoplay: true,
        speed: 1000,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [{
            breakpoint: 992,
            settings: {
                arrows: false,
                dots: true
            }
        },
        {
            breakpoint: 480,
            settings: {
                arrows: false,
                dots: false
            }
        }]
    });


    // product carousel active
    $('.product-carousel-4').slick({
        slidesToShow: 4,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });


    // blog carousel active-2 js
    $('.top-seller-carousel').slick({
        rows: 2,
        arrows: false,
        slidesToShow: 2,
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 992,
                settings: {
                    rows: 1,
                    slidesToShow: 1
                }
            }
        ]
    });


    // blog carousel active-2 js
    $('.blog-carousel-active').slick({
        arrows: false,
        slidesToShow: 3,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });

    // brand slider active js
    $('.brand-active-carousel').slick({
        arrows: false,
        slidesToShow: 4,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });



    // prodct details slider active
    $('.product-large-slider').slick({
        fade: true,
        arrows: false,
        asNavFor: '.pro-nav'
    });


    // product details slider nav active
    $('.pro-nav').slick({
        slidesToShow: 4,
        asNavFor: '.product-large-slider',
        arrows: false,
        focusOnSelect: true
    });
});

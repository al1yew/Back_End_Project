﻿$(document).ready(function () {

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


    //----------------------------------------------- Add to basket

    $(document).on("click", ".addtobasket", function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".minicart-inner-content").html(data);
            })
    })

    //----------------------------------------------- Delete to basket

    $(document).on('click', '.deletefrombasket', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".minicart-inner-content").html(data);
            })
    })

    //----------------------------------------------- Decrease Count of item in basket

    $(document).on('click', '.decrease', function (e) {
        e.preventDefault();
        let inputCount = $(this).next().val();

        if (inputCount >= 1) {
            inputCount--;
            $(this).next().val(inputCount);
            let url = $(this).attr('href') + '/?count=' + inputCount;
            console.log('sub');

            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('.basketindexcontainer').html(data);

                    fetch('/basket/getbasket')
                        .then(res => res.text())
                        .then(data => {
                            $('.minicart-inner-content').html(data);
                        });
                });
        }
    })

    //----------------------------------------------- Increase Count of item in basket

    $(document).on('click', '.increase', function (e) {
        e.preventDefault();
        let inputCount = $(this).prev().val();

        if (inputCount > 0) {
            inputCount++;
        } else {
            inputCount = 1;
        }

        $(this).prev().val(inputCount);

        let url = $(this).attr('href') + '/?count=' + inputCount;
        console.log('add');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);

                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.minicart-inner-content').html(data);
                    });
            });
    })

    //----------------------------------------------- Delete item from basket

    $(document).on('click', '.deletefromcartbtn', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);

                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.minicart-inner-content').html(data);
                    });
            })
    })


    //----------------------------------------------- Add to wishlist

    $(document).on("click", ".addtowishlist", function (e) {

        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".tbodywishlist").html(data);
            })

    })





    //----------------------------------------------- Toastr

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#successInput').val().length) {
        toastr["success"]($('#successInput').val(), $('#successInput').val().split(' ')[0])
    }

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "1000",
        "extendedTimeOut": "500",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#infoinput').val().length) {
        toastr["info"]($('#infoinput').val(), $('#infoinput').val().split(' ')[0])
    }
    //-------------------------------------------------------------------------------------------


    //----------------------------------------------- Tabmenu Navbar

    let path = window.location.pathname
    path = path.split('/')
    let links = $('.header-menu-for-tabmenu')

    for (var i = 0; i < links.length; i++) {
        console.log("djas")
        let hrefpath = links[i].children[0].getAttribute('href').split('/')
        if (hrefpath[1].toLowerCase() == path[1].toLowerCase()) {
            links[i].classList.add('active')
        } else {
            links[i].classList.remove('active')
        }
    }


    //----------------------------------------------- Account Orders toggle table

    $(document).on('show.bs.collapse', '.accordian-body', function () {
        $(this).closest("table")
            .find(".collapse.in")
            .not(this)
        //.collapse('toggle')
    })
});




//----------------------------------------------- GLOBAL GLOBAL GLOBAL --------------------------------------


//----------------------------------------------- Tabmenu Navbar

let path = window.location.pathname
path = path.split('/')
let links = $('.header-menu-for-tabmenu')

for (var i = 0; i < links.length; i++) {
    let hrefpath = links[i].children[0].getAttribute('href').split('/')
    if (hrefpath[1].toLowerCase() == path[1].toLowerCase()) {
        links[i].classList.add('active')
    } else {
        links[i].classList.remove('active')
    }
}


//----------------------------------------------- product details slider active

$('.product-large-slider').slick({
    fade: true,
    arrows: false,
    asNavFor: '.pro-nav'
});


$('.pro-nav').slick({
    slidesToShow: 4,
    asNavFor: '.product-large-slider',
    arrows: false,
    focusOnSelect: true
});

/*
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

});
*/
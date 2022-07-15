$(document).ready(function () {

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




    //----------------------------------------------- Blog page Authors slider

    $('.author-slider').slick();


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

        url = url + '?searchvalue=' + inputvalue;

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

        url = url + '?searchValue=' + inputvalue;

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

    //----------------------------------------------- Delete from basket

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


    //----------------------------------------------- Delete from wishlist

    $(document).on('click', '.deletefromwishlist', function (e) {
        e.preventDefault();
        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.tbodywishlist').html(data);
            })
    })




    //----------------------------------------------- Add to compare

    $(document).on("click", ".addtocompare", function (e) {
        e.preventDefault();
        console.log("hello")

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".comparedata").html(data);
            })
    })


    //----------------------------------------------- Delete from compare

    $(document).on('click', '.deletefromcompare', function (e) {
        e.preventDefault();
        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.comparedata').html(data);
            })
    })




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


    //----------------------------------------------- Account Profile image delete function

    $(document).on("click", ".deleteprofileimage", function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".minicart-inner-content").html(data);
            })
    })


    //----------------------------------------------- Blog comment reply button - adding inputs

    $(document).on("click", ".replybuttonforinputs", function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        console.log(url)

        $('.replybtnforremove').addClass('d-none')

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".replyinputs").html(data);
            })
    })














    //----------------------------------------------- Getting value of sortbycount from select option in products (shop page) index page

    //console.log("dasda")
    //$("#hellolar").change(function () {
    //    console.log($(this).val());
    //    console.log("salam")
    //});


    //$('#hellolar').change(function () {

    //    var select = document.getElementById('hellolar');
    //    var value = select.options[select.selectedIndex].value;
    //    console.log(value)
    //});





















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

*/
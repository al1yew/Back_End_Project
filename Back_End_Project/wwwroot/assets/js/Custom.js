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

});
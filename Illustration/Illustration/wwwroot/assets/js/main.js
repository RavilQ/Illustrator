var priloader = document.getElementById("priloader");

window.addEventListener("load",()=>{
    priloader.style.display="none";
})


let searchBtn = document.getElementById("my-search-button");
let navbar = document.getElementById("my-navbar");
let searchInput = document.getElementById("my-search-input");


searchBtn.addEventListener("click",()=>{
   navbar.style.display="none";
   searchInput.style.display="block";
})

document.addEventListener('click', function(event) {
    var e=document.getElementById('my-search-button');
    var b=document.getElementById("my-search-input");
    if (!e.contains(event.target) && !b.contains(event.target)){
        navbar.style.display="block";
        searchInput.style.display = "none";

        let link = `/Portrait/Search?words=${null}`

        fetch(link)
            .then(response => {
                return response.text();
            }).then(html => {
                $("#mySearchContainer").html(html)
            })
    }
});

// var scrollsearchBtn = document.getElementById("my-scroll-search-button");
// var scrollnavbar = document.getElementById("my-scroll-navbar");
// var scrollsearchInput = document.getElementById("my-scroll-search-input");


// scrollsearchBtn.addEventListener("click",()=>{
//    scrollnavbar.style.display="none";
//    scrollsearchInput.style.display="block";
// })

// document.addEventListener('click', function(event) {
//     var e=document.getElementById('my-scroll-search-button');
//     var b=document.getElementById("my-scroll-search-input");
//     if (!e.contains(event.target) && !b.contains(event.target)){
//         scrollnavbar.style.display="block";
//         scrollsearchInput.style.display="none";
//     }
// });


var prevScrollpos = window.pageYOffset;
window.onscroll = function() {
var currentScrollPos = window.pageYOffset;
  if (prevScrollpos > currentScrollPos) {
    document.getElementById("my-scroll-navbar").style.top = "0";
  } else {
    document.getElementById("my-scroll-navbar").style.top = "-60px";
  }
  if(currentScrollPos==0){
    document.getElementById("my-scroll-navbar").style.display = "none";
  }
  else{
    document.getElementById("my-scroll-navbar").style.display = "block";
  }
  prevScrollpos = currentScrollPos;

  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    document.getElementById("upBtn").style.display = "block";
  } else {
    document.getElementById("upBtn").style.display = "none";
  }
  if (document.body.scrollTop > 3000 || document.documentElement.scrollTop > 3000) {
    document.getElementById("scrollbtnicon").style.color = "black";
    document.getElementById("upBtn").style.backgroundColor = "white";
  } else {
    document.getElementById("scrollbtnicon").style.color = "white";
    document.getElementById("upBtn").style.backgroundColor = "black";
  }
}

$(document).ready(function() {
  $('#heart-icon').click(function() {
    // $('#wishlist').css('display', 'block');
    $('#wishlist').css('right', '0');
  });
  $('#wishlist').mouseleave(function() {
    $(this).css('right', '-25%');
    setTimeout(main, 5000); 
  });
});

$(document).ready(function() {
  $('#heart-icon2').click(function() {
    $('#wishlist').css('right', '0');
  });
  $('#wishlist').mouseleave(function() {
    $(this).css('right', '-25%');
    setTimeout(main, 1000); 
  });
});

$(document).ready(function() {
  if (wishlistCheck==true) {
    setTimeout(main, 1000); 
  }
});

// function main() {
//   $('#wishlist').css('display','none');
// }

document.getElementById("upBtn").addEventListener("click", function() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
});


//$(function () {
//  $("#slider-range").slider({
//    range: true,
//    min: 130,
//    max: 500,
//    values: [130, 250],
//    slide: function (event, ui) {
//      $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
//    }
//  });
//  $("#amount").val(
//    "$" +
//      $("#slider-range").slider("values", 0) +
//      " - $" +
//      $("#slider-range").slider("values", 1)
//  );
//});


$(function(){
  $(".heading-compose").click(function() {
    $(".side-two").css({
      "left": "0"
    });
  });

  $(".newMessage-back").click(function() {
    $(".side-two").css({
      "left": "-100%"
    });
  });
}) 





$(document).on("click", ".add-to-wishlist", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {
            return response.text();
        }).then(html => {
            toastr["success"]("Added to WishList !!")
            $("#wishlist-block").html(html)
        })
})




$(document).on("click", ".DeleteToWishList", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {
            return response.text();
        }).then(html => {
            $("#wishlist-block").html(html)
        })
})



toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": false,
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



$(document).on("click", '.my-problemly-raitings', function (e) {
    window.scrollTo(0, 1750);
});


function mySearchFunction() {

    var myLayoutSearch = document.getElementById("my-search-input")
    var values = myLayoutSearch.value.split(" ").join("20%")
    let link = `/Portrait/Search?words=${values}`

    fetch(link)
        .then(response => {
            return response.text();
        }).then(html => {
            $("#mySearchContainer").html(html)
        })
}


//$(document).on("click", '.myaccount-delete-icon-a', () => {
//    e.preventDefault();
//    let url = $(this).attr("href");

//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        iconColor: 'black',
//        showCancelButton: true,
//        confirmButtonColor: 'black',
//        confirmButtonBorder: 'none',
//        cancelButtonColor: 'black',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            fetch(url)
//                .then(response => {
//                    if (response.ok) {
//                        Swal.fire(
//                            'Deleted!',
//                            'Your file has been deleted.',
//                            'success'
//                        ).then(() => window.location.reload())
//                    }
//                    else {
//                        Swal.fire({
//                            icon: 'error',
//                            title: 'Slider not found!',
//                            text: 'Something went wrong!',
//                        })
//                    }
//                })
           
//        }
//    })
//})

$(document).on("click", ".myaccount-delete-icon-a", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        iconColor: 'black',
        showCancelButton: true,
        confirmButtonColor: 'black',
        confirmButtonBorder: 'none',
        cancelButtonColor: 'black',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(url)
                .then(response => {
                    if (response.ok) {
                        Swal.fire({
                            icon: 'success',
                            iconColor: 'black',
                            title: 'Deleted!',
                            text: "Your file has been deleted.",
                            confirmButtonColor: 'black'
                        }).then(() => window.location.reload())
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Portrait not found!',
                            text: 'Something went wrong!',
                        })
                    }
                })
        }
    })
})

let connection = new signalR.HubConnectionBuilder().withUrl("/illustratorhub").build();

connection.start().then(function () {

})

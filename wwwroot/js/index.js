

const showNavBar = (toggleId, navId, bodyId, headerID) => {
    const toggle = document.getElementById(toggleId),
    nav = document.getElementById(navId),
    bodypd = document.getElementById(bodyId),
    headerpd = document.getElementById(headerID)

    if(toggle && nav && bodypd && headerpd)
    {
        toggle.addEventListener('click', () =>{
            nav.classList.toggle('show');

            toggle.classList.toggle('fa-times');

            bodypd.classList.toggle('body-pd');

            headerpd.classList.toggle('body-pd');
        })
    }
}

showNavBar('header-toggle', 'nav-bar', 'body-pd','header');

const linkColor = document.querySelectorAll('.nav__link');

function colorLink(){
    if(linkColor){
        linkColor.forEach(l => l.classList.remove('active'))
        this.classList.add('active')
    }
}

linkColor.forEach(l => l.addEventListener('click', colorLink))

var tabs = document.querySelectorAll(".tabs ul li");
var tabs_wrap = document.querySelectorAll(".tab_wrap");

tabs.forEach(function(tab, tab_index){
    tab.addEventListener("click", function(){
        tabs.forEach(function(tab){
            tab.classList.remove("active")
        })

        tab.classList.add("active");

        tabs_wrap.forEach(function(content, content_index){
            if(content_index == tab_index){
                content.style.display = "block";
            }
            else{
                content.style.display = "none";
            }
        })
    })
})


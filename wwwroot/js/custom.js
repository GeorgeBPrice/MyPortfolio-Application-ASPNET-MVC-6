/* Custom JS */

// This is the scripts required to animate the skills progress bars
// selecting all elements
// using an event and scroll listerner
// When user scrolls to "Skills" section, the mapper will animate the progess

$(document).ready(function ($) {
    "use strict";
    /**
     * Easy selector helper function
     */

    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    /**
     * Easy event listener function
     */
    const on = (type, el, listener, all = false) => {
        let selectEl = select(el, all)
        if (selectEl) {
            if (all) {
                selectEl.forEach(e => e.addEventListener(type, listener))
            } else {
                selectEl.addEventListener(type, listener)
            }
        }
    }

    /**
     * Easy on scroll event listener 
     */
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    /**
    * Skills animation
    */
    let skilsContent = select('.skills-content');
    if (skilsContent) {
        new Waypoint({
            element: skilsContent,
            offset: '80%',
            handler: function (direction) {
                let progress = select('.progress .progress-bar', true);
                progress.forEach((el) => {
                    el.style.width = el.getAttribute('aria-valuenow') + '%'
                });
            }
        })
    }

    // Other MISC improvements

    /**
     * close responsive mobile/tablet main nav when a link is clicked
     */
    $('.navbar-nav>li>a').on('click', function () {
        $('.navbar-collapse').collapse('hide');
    });

    /**
     * toggle 'show more' technology icons => | up | down |
     */
    $('#icon-toggle').click(function () {
        $(this).find('i').toggleClass('ti-angle-down').toggleClass('ti-angle-up');
    });

});
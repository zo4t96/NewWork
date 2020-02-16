//可拖動Ul/Li物件初始化
var drag = {
class_name: null,
permitDrag: false,
_x: 0,
_y: 0,
_left: 0,
_top: 0,
old_elm: null,
tmp_elm: null,
new_elm: null,
init: function (className) {
drag.class_name = className;
$('.' + drag.class_name).on('mousedown', 'ul li', function (event) {
                drag.permitDrag = true;
                drag.old_elm = $(this);
                drag.mousedown(event);
                return false;
});
$(document).mousemove(function (event) {
                if (!drag.permitDrag) return false;
                drag.mousemove(event);
                return false;
});
$(document).mouseup(function (event) {
                if (!drag.permitDrag) return false;
                drag.permitDrag = false;
                drag.mouseup(event);
                return false;
});
        },
mousedown: function (event) {

          //  console.log('我被mousedown了');
            drag.tmp_elm = $(drag.old_elm).clone();

            drag._x = $(drag.old_elm).offset().left;
            drag._y = $(drag.old_elm).offset().top;

            var e = event || window.event;
            drag._left = e.pageX - drag._x;
            drag._top = e.pageY - drag._y;

            $(drag.tmp_elm).css({
                'position': 'absolute',
                'background-color': '#FF8C69',
                'left': drag._x,
                'top': drag._y,
            });

            tmp = $(drag.old_elm).parent().append(drag.tmp_elm);
            drag.tmp_elm = $(tmp).find(drag.tmp_elm);
            $(drag.tmp_elm).css('cursor', 'move');

        },

        mousemove: function (event) {

          //  console.log('我被mousemove了');

            var e = event || window.event;
            var x = e.pageX - drag._left;
            var y = e.pageY - drag._top;
            var maxL = $(document).width() - $(drag.old_elm).outerWidth();
            var maxT = $(document).height() - $(drag.old_elm).outerHeight();

            x = x < 0 ? 0 : x;
            x = x > maxL ? maxL : x;
            y = y < 0 ? 0 : y;
            y = y > maxT ? maxT : y;

            $(drag.tmp_elm).css({
                'left': x,
                'top': y,
            });

            $.each($('.' + drag.class_name + ' ul'), function (index, value) {

                var box_x = $(value).offset().left;
                var box_y = $(value).offset().top;
                var box_width = $(value).outerWidth();
                var box_height = $(value).outerHeight();

                if (e.pageX > box_x && e.pageX < box_x - 0 + box_width && e.pageY > box_y && e.pageY < box_y - 0 + box_height) {

                    if ($(value).offset().left !== drag.old_elm.parent().offset().left
                        || $(value).offset().top !== drag.old_elm.parent().offset().top) {

                        $(value).css('background-color', '#FFEFD5');
                    }
                } else {
                    $(value).css('background-color', '#FFFFF0');
                }

            });

        },

        mouseup: function (event) {

          //  console.log('我被mouseup了');
            $(drag.tmp_elm).remove();

            var e = event || window.event;

            $.each($('.' + drag.class_name + ' ul'), function (index, value) {

                var box_x = $(value).offset().left;
                var box_y = $(value).offset().top;
                var box_width = $(value).outerWidth();
                var box_height = $(value).outerHeight();

                if (e.pageX > box_x && e.pageX < box_x - 0 + box_width && e.pageY > box_y && e.pageY < box_y - 0 + box_height) {

                    if ($(value).offset().left !== drag.old_elm.parent().offset().left
                        || $(value).offset().top !== drag.old_elm.parent().offset().top) {
                        tmp = $(drag.old_elm).clone();
                        var newObj = $(value).append(tmp);
                        $(drag.old_elm).remove();
                        drag.new_elm = $(newObj).find(tmp);
                    }
                }
                $(value).css('background-color', '#FFFFF0');
            });

        },

    };
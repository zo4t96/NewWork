        function changepage(link) {
            $.post(link, {ajax:true}, function (data) {
                $(".mainbox").html(data);
            })
        }
$("li > a").on("click", function (e) {
    let link = $(this).attr("href");
    //var $this = $(this),
    //    url = $this.attr("href"),
    //title = $this.text();
    //history.pushState({
	   //         url: url,
    //    title: title,
    //            ajax:true
    //}, title, url);
    //document.title = title;
  //  changepage(url);
    e.preventDefault();
    changepage(link);
           history.pushState(null,null, link);

        })
        window.addEventListener('popstate', function (e) {
        //    location.href = location.pathname;
           //        changepage(location.pathname);
     //       changepage(this.history.state.url);

            if (history.state == null) {
                history.replaceState({ ajax: true }, null);
            }
            $.post(location.pathname, history.state, function (data) {
                $(".mainbox").html(data);
            })
            console.log(history.state);

        })
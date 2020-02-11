        function changepage(link) {
            $.post(link, {ajax:true}, function (data) {
                $(".mainbox").html(data);
            })
        }
$("a").on("click", function (e) {
//    let link = $(this).attr("href");
    var $this = $(this),
    url = $this.attr("href"),
    title = $this.text();
    history.pushState({
	            url: url,
	            title: title
    }, title, url);
    document.title = title;
    changepage(url);
    e.preventDefault();
//    changepage(link);
       //    history.pushState(null,null, link);

        })
        window.addEventListener('popstate', function (e) {
        //    location.href = location.pathname;
                   changepage(location.pathname);
     //       changepage(this.history.state.url);
        })
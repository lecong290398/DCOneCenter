
    var urlProjects = 'DCOneCenter/LoadDataProjectIndex';
    $.ajax({
        url: urlProjects,
        type: 'POST',
        processData: false,
        contentType: false,
    })
        .done(function (resp) {
            if (resp.success && resp.result.dataProjects) {

                if (resp.result.dataProjects.promoteD_PROJECTS.length > 0) {
                    for (var i = 0; i < resp.result.dataProjects.promoteD_PROJECTS.length; i++) {
                        var data = resp.result.dataProjects.promoteD_PROJECTS;
                        var html = '<div role="listitem" class="homepromote w-dyn-item">' +
                            '                            <a href="projects/numetic.html"' +
                            '                           class="homecard w-inline-block">' +
                            '                                <div class="cardheader">' +
                            '                                    <div style="background-image:url(' + data[i].logo + ')"' +
                            '                                     class="cardprojectimage"></div>' +
                            '                                    <h3 class="cardprojecttitle">' + data[i].projectName +
                            '                                </h3></div>' +
                            '                                <div class="cardmain">' +
                            '                                    <p class="projectcardparagraph">' +
                            '                                      ' + data[i].projectSummary +
                            '                                    </p>' +
                            '                                </div>' +
                            '                                <div class="cardfooter w-clearfix">' +
                            '                                    <div class="button">Learn more</div>' +
                            '                                </div>' +
                            '                            </a>' +
                            '                        </div>';

                        $("#PROMOTED_PROJECTS_DATA").append(html);
                    }
                }


                if (resp.result.dataProjects.populaR_PROJECTS.length > 0) {
                    for (var i = 0; i < resp.result.dataProjects.populaR_PROJECTS.length; i++) {
                        var data = resp.result.dataProjects.populaR_PROJECTS;
                        var html = '<div role="listitem" class="homepromote w-dyn-item">' +
                            '                            <a href="projects/numetic.html"' +
                            '                           class="homecard w-inline-block">' +
                            '                                <div class="cardheader">' +
                            '                                    <div style="background-image:url(' + data[i].logo + ')"' +
                            '                                     class="cardprojectimage"></div>' +
                            '                                    <h3 class="cardprojecttitle">' + data[i].projectName +
                            '                                </h3></div>' +
                            '                                <div class="cardmain">' +
                            '                                    <p class="projectcardparagraph">' +
                            '                                      ' + data[i].projectSummary +
                            '                                    </p>' +
                            '                                </div>' +
                            '                                <div class="cardfooter w-clearfix">' +
                            '                                    <div class="button">Learn more</div>' +
                            '                                </div>' +
                            '                            </a>' +
                            '                        </div>';

                        $("#POPULAR_PROJECTS_DATA").append(html);
                    }
                }


                if (resp.result.dataProjects.recentlY_ADDED.length > 0) {
                    for (var i = 0; i < resp.result.dataProjects.recentlY_ADDED.length; i++) {
                        var data = resp.result.dataProjects.recentlY_ADDED;
                        var html = '<div role="listitem" class="homepromote w-dyn-item">' +
                            '                            <a href="projects/numetic.html"' +
                            '                           class="homecard w-inline-block">' +
                            '                                <div class="cardheader">' +
                            '                                    <div style="background-image:url(' + data[i].logo + ')"' +
                            '                                     class="cardprojectimage"></div>' +
                            '                                    <h3 class="cardprojecttitle">' + data[i].projectName +
                            '                                </h3></div>' +
                            '                                <div class="cardmain">' +
                            '                                    <p class="projectcardparagraph">' +
                            '                                      ' + data[i].projectSummary +
                            '                                    </p>' +
                            '                                </div>' +
                            '                                <div class="cardfooter w-clearfix">' +
                            '                                    <div class="button">Learn more</div>' +
                            '                                </div>' +
                            '                            </a>' +
                            '                        </div>';

                        $("#RECENTLY_ADDED_DATA").append(html);
                    }
                }

            } else {
                this.message.error(resp.result.message);
            }
        })
        .always(function () {

        });


var urlProjectsIndustres = 'DCOneCenter/LoadDataProjectIndustreIndex';

$.ajax({
    url: urlProjectsIndustres,
    type: 'POST',
    processData: false,
    contentType: false,
})
    .done(function (resp) {
        if (resp.success && resp.result.dataProjectsIndustres) {
            
            if (resp.result.dataProjectsIndustres.length > 0) {

                for (var i = 0; i < resp.result.dataProjectsIndustres.length; i++) {


                    var html = '<div role="listitem" class="w-dyn-item">' +
                        '                        <a href="collections/community.html"' +
                        '                           class="cardcategory w-inline-block">' +
                        '                            <img src="/view-resources/Views/DCOneCenter/ImagesProjectIndustries/' + resp.result.dataProjectsIndustres[i].projectIndustrie.logo+'.png"' +
                        '                                 loading="lazy" alt="" class="collectionicon" />' +
                        '                            <h1 class="collectiontitle">' + resp.result.dataProjectsIndustres[i].projectIndustrie.nameIndustries +'</h1>' +
                        '                        </a>' +
                        '                    </div>';

;

                    $("#DataProjectIndutres").append(html);

                }
            }
        } else {
            this.message.error(resp.result.message);
        }
    })
    .always(function () {

    });


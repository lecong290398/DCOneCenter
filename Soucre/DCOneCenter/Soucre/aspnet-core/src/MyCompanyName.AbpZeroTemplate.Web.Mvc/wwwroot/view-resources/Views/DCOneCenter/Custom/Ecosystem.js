
var urlEcosystem = 'LoadDataEcosystemPage';

$.ajax({
    url: urlEcosystem,
    type: 'POST',
    processData: false,
    contentType: false,
})
    .done(function (resp) {
        if (resp.success && resp.result.dataProjectsIndustres) {
            
            if (resp.result.dataProjectsIndustres.length > 0) {

                for (var i = 0; i < resp.result.dataProjectsIndustres.length; i++) {



                    var header = '<div class="category">' +
                        '          <div class="mapheader">' +
                        '            <h2 class="projecttitlemap nftmap">' + resp.result.dataProjectsIndustres[i].nameIndustres + '</h2>' +
                        '          </div>' +
                        '          <div class="w-dyn-list">' +
                        '            <div role="list" class="columngrid nftmarketplaces w-dyn-items">' +
                        '';

                    var body = '';
                    for (var j = 0; j < resp.result.dataProjectsIndustres[i].listProject.length; j++) {
                        var contenbody = '  <div role="listitem" class="collection-item-5 w-dyn-item">' +
                            '                  <a href="projects/paradiso.html" class="projectlinkblock w-inline-block">' +
                            '                      <div' +
                            '                        style="background-image:url(/view-resources/Views/DCOneCenter/ImagesProject/' + resp.result.dataProjectsIndustres[i].listProject[j].logo+')"' +
                            '                        class="div-block-27">' +
                            '                        <div class="token-sale-nft-label w-condition-invisible"></div>' +
                            '                      </div>' +
                            '                      <h3 class="ecosystemtitle">' + resp.result.dataProjectsIndustres[i].listProject[j].nameProject+'</h3>' +
                            '                    </a>' +
                            '                </div>';
                        body = body + contenbody;
                    }
                    
                       


                       var Footer =  '             ' +
                        '            </div>' +
                        '          </div>' +
                        '        </div>';

                    var html = header + body + Footer;
                         
                    $("#ImportCategory").append(html);

                }
            }
        } else {
            this.message.error(resp.result.message);
        }
    })
    .always(function () {

    });


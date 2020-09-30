$(document).ready(function () {
    /*
    *  CATEGORIE SUPPRESSION
    */

    /*
     * Stocke l'Id du livre dans la zone cachée
     */
    $(".categorie").click(function (e) {
        e.preventDefault();
        var categorieId = $(this).attr('data-categorieid');
        console.log('categorieId', categorieId);

        //copy de bookId vers la zone cachée
        $('#suppresse-categorieId').val(categorieId);

    });

    /*
     *  Au click sur le bouton suppression de la fenêtre modale de confirmation
     *  Envoie de la requete de suppression vers le serveur de la categorie dont l'Id est stocké dans la zone cachée
     */
    $('#btnSuppressionCategorie').on('click', () => {
        console.log('demande de suppression');
        var categorieId = $('#suppresse-categorieId').val();

        //envoie de la requete de suppression vers le controlleur BookController avec l'action delete et avec comme parametre l'Id du livre sur lequel on a cliqué
        $.get(`delete/${categorieId}`, function (status) {
            console.log("Status: " + status);
            if (status == 200) {
                //hide
                $(`tr[data-categorieid=${categorieId}]`).hide(1000);
            }
        });

        //on ferme la fenêtre modale
        $('#deleteModalCategorie').modal('hide');

    });




    /* ================================================================================================================ */

    /*
     *  BOOK SUPPRESSION
     */ 

    /*
     * Stocke l'Id du livre dans la zone cachée
     */
    $(".book").click(function (e) {
        e.preventDefault();
        var bookId = $(this).attr('data-bookid');
        console.log('bookId', bookId);

        //copy de bookId vers la zone cachée
        $('#suppresse-bookId').val(bookId);
    });

    /*
     *  Au click sur le bouton suppression de la fenêtre modale de confirmation
     *  Envoie de la requete de suppression vers le serveur du livre dont l'Id est stocké dans la zone cachée
     */
    $('#btnSuppression').on('click', () => {
        console.log('demande de suppression');
        var bookId = $('#suppresse-bookId').val();

        // envoie de la requete de suppression vers le controlleur BookController avec l'action delete 
        // et avec comme parametre l'Id du livre sur lequel on a cliqué
        $.get(`delete/${bookId}`, function (status) {
            console.log("Status: " + status);
            if (status == 200) {
                //hide
                $(`tr[data-bookid=${bookId}]`).hide(1000);
            }
        });

        //on ferme la fenêtre modale
        $('#deleteModal').modal('hide');
       
    });




});
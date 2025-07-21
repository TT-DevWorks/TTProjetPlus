/**
 *name: jquery.highCheckTree
 *author: yan, xuekai
 *version: 0.2.1
*/

/**input data format: 

[{
    item:{id:'id', label:'label', checked:false}, 
    chidren:[{
        item:{id:'id', label:'label', checked:false}, 
        chidren:[...]
    }]
}, ....]

*/

(function ($) {

    jQuery.fn.highCheckTree = function (settings) {

        settings = $.extend({
            data: [],               // input data which will be used to initilze the tree
            onExpand: null,        // an event will be triggered when the tree node was expanded
            onCollapse: null,      // an event will be triigered when the tree node was collapsed
            onPreCheck: null,      // an event will be triggered before the tree node was checked
            onCheck: null,         // an event will be triggered when the tree node was checked
            onUnCheck: null,       // an event will be triggered when the tree node was unchecked
            onLabelHoverOver: null,// an event will be triggered when mouse hover over the label
            onLabelHoverOut: null  // an event will be triggered when mouse hover out the label
        }, settings);

        var container = $(this), $tree = this;

        //get children html tag string
        function getChildrenHtml(treesdata) {
            var result = '',
                len = treesdata.children == undefined ? treesdata.length : treesdata.children.length,
                node,
                clen,
                arrowClass,
                //fred:
                checkedClass = ''
            checkedChildren;
            for (i = 0; i < len; i++) {
                node = treesdata.children == undefined ? treesdata[i] : treesdata.children[i];
                $.data($tree, node.key, node); //attach node data to node id
                clen = node.children ? node.children.length : 0;
                arrowClass = 'collapsed';
                if (clen === 0) {
                    arrowClass = 'nochildren';
                    checkClass = node.Checked ? 'checked' : '';
                }
                //ajout de fred
                else if (clen != 0 && node.Checked) {
                    checkClass = 'checked';
                }

                else {
                    var checkedChildren = $.grep(node.children, function (el) {
                        return el.Checked;
                    });
                    checkClass = checkedChildren.length === 0 ? '' : checkedChildren.length === clen ? 'checked' : 'half_checked';
                }

                var noCheck = node.Disabled123 ? "noCheckboxTree" : "";
                var disableAll = node.AllDisabled ? "disablingCheck" : "";
                var folderCheckDisabled = node.FolderCheckDisabled ? "checkDisabled" : "";
                if (noCheck == "noCheckboxTree") {
                    result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class=" disablingCheck " ></div><label  style="color:red">' +
                        "\xa0\xa0\xa0\xa0\xa0\xa0" + node.name + '</label></li>';

                }
                else {
                    if (disableAll == "disablingCheck")
                    {
                        if (checkClass == "checked")
                        {
                            result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class="checkboxTree disablingCheck ' + checkClass + '"></div><label style="color:red">' +
                                node.name + '</label></li>';

                        }
                        else
                        {
                        result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class="checkboxTree disablingCheck ' + checkClass + '"></div><label>' +
                            node.name + '</label></li>';

                        }

                    }
                    else
                    {
                        if (folderCheckDisabled == "checkDisabled") {
                            result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class=" disablingCheck " ></div><label style="color:red">' + //
                                "\xa0\xa0\xa0\xa0\xa0\xa0"+ node.name + '</label></li>';
                        }
                        else

                        {
                            if (checkClass == "checked")
                            {
                                result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class="checkboxTree ' + checkClass + '"></div><label style="">' + //pahes 2: <label style="color:green">
                                node.name + '</label></li>';

                            }
                            else
                            {
                                result += '<li rel="' + node.key + '"><div class="arrow ' + arrowClass + '"></div><div class="checkboxTree ' + checkClass + '"></div><label style="">' +
                                    node.name + '</label></li>';

                            }

                        }


                    }

                }
            }

            return result;
        }

        //display children node with data source
        function updateChildrenNodes($li, data, isExpanded) {
            if (data.children && data.children.length > 0) {
                var innerHtml = isExpanded ? '<ul>' : '<ul style="display:none;">';
                innerHtml += getChildrenHtml(data.children) + '</ul>';
                $li.append(innerHtml);
            }

            $li.addClass('updated');
        }

        //initialize the check tree with the input data
        (function initalCheckTree() {
            var treesHtml = '<ul class="checktree">';
            treesHtml += getChildrenHtml(settings.data);
            container.empty().append(treesHtml + '<ul>');
        })();

        //bind select change to checkboxTree
        container.off('selectchange', '.checkboxTree').on('selectchange', '.checkboxTree', function () {
            if (settings.onPreCheck) {
                if (!settings.onPreCheck($(this).parent())) {
                    return;
                }
            }

            var $li = $(this).parent();
            var dataSource = $.data($tree, $li.attr('rel'));
            var $all = $(this).siblings('ul').find('.checkboxTree');
            var $checked = $all.filter('.checked');

            //all children checked
            if ($all.length === $checked.length) {
                $(this).removeClass('half_checked').addClass('checked');
                dataSource.Checked = true;
                if (settings.onCheck) {
                    settings.onCheck($li);
                }
                //all children are unchecked
            } else if ($checked.length === 0) {
                dataSource.Checked = false;
             //fred  //   $(this).removeClass('checked').removeClass('half_checked');
                if (settings.onUnCheck) {
                    settings.onUnCheck($li);
                }
                //some children are checked
            } else {
                dataSource.Checked = false;
                if (settings.onHalfCheck && !$(this).hasClass('half_checked')) {
                    settings.onHalfCheck($li);
                }

             //fred  // $(this).removeClass('checked').addClass('half_checked');
                $(this).addClass('checked');
            }
        });

        //expand and collapse node
        container.off('click', '.arrow').on('click', '.arrow', function () {
            if ($(this).hasClass('nochildren')) {
                return;
            }

            var $li = $(this).parent();
            if (!$li.hasClass('updated')) {
                updateChildrenNodes($li, $.data($tree, $li.attr('rel')), true);
                $(this).removeClass('collapsed').addClass('expanded');
                if (settings.onExpand) {
                    settings.onExpand($li);
                }
            } else {
                $(this).siblings("ul").toggle();
                if ($(this).hasClass('collapsed')) {
                    $(this).removeClass('collapsed').addClass('expanded');
                    if (settings.onExpand) {
                        settings.onExpand($li);
                    }
                } else {
                    $(this).removeClass('expanded').addClass('collapsed');
                    if (settings.onCollapse) {
                        settings.onCollapse($li);
                    }
                }
            }
        });

        //check and uncheck node
        container.off('click', '.checkboxTree').on('click', '.checkboxTree', function () {
            var $li = $(this).parent();
            var dataSource = $.data($tree, $li.attr('rel'));
            if (!$li.hasClass('updated')) {
                updateChildrenNodes($li, dataSource, false);
            }

            if (settings.onPreCheck) {
                if (!settings.onPreCheck($li)) {
                    return;
                }
            }

            $(this).removeClass('half_checked').toggleClass('checked');

            if ($(this).hasClass('checked')) {
                dataSource.Checked = true;
                if (settings.onCheck) {
                    settings.onCheck($li, true);
                }
                //cette partie du code est commentée pour permettre de cocher un parent sans que ses enfants soient cochés
                //a moins de le faire explicitement              

                //$(this).siblings('ul').find('.checkboxTree').not('.checked').removeClass('half_checked').addClass('checked').each(function () {
                //    var $subli = $(this).parent();
                //    $.data($tree, $subli.attr('rel')).Checked = true;
                //    if (settings.onCheck) {
                //        settings.onCheck($subli, false);
                //    }
                //});
            } else {
                dataSource.Checked = false;
                if (settings.onUnCheck) {
                    settings.onUnCheck($li, true);
                }

                $(this).siblings('ul').find('.checkboxTree').filter('.checked').removeClass('half_checked').removeClass('checked').each(function () {
                    var $subli = $(this).parent();
                    $.data($tree, $subli.attr('rel')).Checked = false;
                    if (settings.onUnCheck) {
                        settings.onUnCheck($subli, false);
                    }
                });
            }

            $(this).parents('ul').siblings('.checkboxTree').trigger('selectchange');
        });

        //click label also trigger check action
        container.off('click', 'label').on('click', 'label', function () {
            $(this).prev('.checkboxTree').trigger('click');
        });

        container.off('mouseenter', 'label').on('mouseenter', 'label', function () {
            $(this).addClass("hover");
            if (settings.onLabelHoverOver) settings.onLabelHoverOver($(this).parent());
        });

        container.off('mouseleave', 'label').on('mouseleave', 'label', function () {
            $(this).removeClass("hover");
            if (settings.onLabelHoverOut) settings.onLabelHoverOut($(this).parent());
        });
    };
})(jQuery);

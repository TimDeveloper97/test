import { Component, HostBinding, Input } from '@angular/core';

import { NavigationItem } from '../navigation/navigationitem';

@Component({
    selector   : 'nav-item',
    templateUrl: './item.component.html',
    styleUrls  : ['./item.component.scss']
})
export class NavItemComponent
{
    @Input()
    item: NavigationItem;

    /**
     * Constructor
     */
    constructor()
    {
    }
}

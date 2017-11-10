import { AfterContentInit, Component, ContentChildren, QueryList } from '@angular/core';
import { TabComponent } from './tab/tab.component';

@Component({
    selector: 'tabs',
    styleUrls: ['./tabs.component.scss'],
    templateUrl: './tabs.component.html'
})
export class TabsComponent implements AfterContentInit {
    @ContentChildren(TabComponent) tabs: QueryList<TabComponent>;

    public ngAfterContentInit() {
        const activeTabs = this.tabs.filter((tab) => tab.active);

        if (activeTabs.length === 0) {
            this.selectTab(this.tabs.first);
        }
    }

    public selectTab(tab: TabComponent) {
        this.tabs.toArray().forEach(e => e.active = false);
        tab.active = true;
    }
}

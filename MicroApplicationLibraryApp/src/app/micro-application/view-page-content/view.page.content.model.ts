export class PageContentView {
    public Contents: PagePanelContentView[] = [];
}

export class PagePanelContentView {
    public Title: string ="";
    public PageContentItemViews: PageContentItemView[] = [];

}

export class PageContentItemView {
    public Label?: string="";
    public Value?: string="";
    public Column: number=0;
    public IsSingleLine: boolean= false;
}
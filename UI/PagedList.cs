using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PagedList : VisualElement {

    private SortableList list;

    private Button prevPageBtn, nextPageBtn;
    private Label currentPageLabel;

    public event Action OnPrevPageClicked;
    public event Action OnNextPageClicked;

    public PagedList(SortableList list) {

        this.list = list;

        var pageNavContainer = new VisualElement();
        pageNavContainer.AddToClassList("paged-list-nav-container");

        pageNavContainer.style.flexDirection = FlexDirection.Row;
        pageNavContainer.style.justifyContent = Justify.Center;
        pageNavContainer.style.alignItems = Align.Center;

        prevPageBtn = new Button { text = "Prev" };
        prevPageBtn.AddToClassList("paged-list-btn");
        prevPageBtn.clicked += () => OnPrevPageClicked?.Invoke();

        nextPageBtn = new Button { text = "Next" };
        nextPageBtn.AddToClassList("paged-list-btn");
        nextPageBtn.clicked += () => OnNextPageClicked?.Invoke();

        currentPageLabel = new Label("1");
        currentPageLabel.AddToClassList("paged-list-label");

        pageNavContainer.Add(prevPageBtn);
        pageNavContainer.Add(currentPageLabel);
        pageNavContainer.Add(nextPageBtn);

        Add(list);
        Add(pageNavContainer);

    }

    public void SetPageLabel(string newLabel) {
        currentPageLabel.text = newLabel;
    }

}

public class PagedListControl<T> {

    private SortableListControl<T> list;

    private int currentPage = 1;
    private int maxPages = 1;

    public int pageSize = 10;

    public PagedList view { get; private set; }

    public event Action<T> OnSingleClick;
    public event Action<T> OnDoubleClick;

    public event Action<int> OnPageChange;

    public PagedListControl(List<string> columnNames, List<Func<T, IComparable>> columnSelectors) {

        list = new SortableListControl<T>(columnNames, columnSelectors);
        list.BindView();

        list.OnSingleClick += (item) => OnSingleClick?.Invoke(item);
        list.OnDoubleClick += (item) => OnDoubleClick?.Invoke(item);

        view = new PagedList(list.view);
        view.SetPageLabel($"{currentPage} of {maxPages}");

        view.OnPrevPageClicked += () => {
            if(currentPage > 1) {
                currentPage--;
                view.SetPageLabel($"{currentPage} of {maxPages}");
                OnPageChange?.Invoke(currentPage);
            }
        };

        view.OnNextPageClicked += () => {
            if(currentPage < maxPages) {
                currentPage++;
                view.SetPageLabel($"{currentPage} of {maxPages}");
                OnPageChange?.Invoke(currentPage);
            }
        };

    }

    public void SetItems(List<T> items) {
        list.SetItems(items);
    }

    public void ClearRows() {
        list.ClearRows();
    }

    public void SetMaxPage(int maxPage) {
        this.maxPages = maxPage;
        view.SetPageLabel($"{currentPage} of {maxPages}");
    }

    public T GetLastSelected() {
        return list.GetLastSelected();
    }

}

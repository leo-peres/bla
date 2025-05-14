using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

public class LinkedList<T> {

    public LinkedListElement<T> head, tail;

    public int size;

    public LinkedList() {

        head = new LinkedListElement<T>();
        tail = new LinkedListElement<T>();

        head.next = tail;
        tail.prev = head;

        size = 0;

    }

    public LinkedListElement<T> InsertAfter(T value, LinkedListElement<T> prev) {
        
        LinkedListElement<T> newElement = new LinkedListElement<T>(value);

        newElement.next = prev.next;
        newElement.prev = prev;
        prev.next = newElement;
        newElement.next.prev = newElement;

        size++;

        return newElement;

    }

    public LinkedListElement<T> InsertBefore(T value, LinkedListElement<T> next) {

        LinkedListElement<T> newElement = new LinkedListElement<T>(value);

        newElement.next = next;
        newElement.prev = next.prev;
        next.prev = newElement;
        newElement.prev.next = newElement;

        size++;

        return newElement;

    }

    public LinkedListElement<T> Insert(T value) {
        return InsertAfter(value, head);
    }

    public LinkedListElement<T> Push(T value) {
        return InsertBefore(value, tail);
    }

    public void Remove(LinkedListElement<T> oldElement) {
        
        if(oldElement.Equals(head) || oldElement.Equals(tail))
            return;
        
        oldElement.prev.next = oldElement.next;
        oldElement.next.prev = oldElement.prev;

        size--;

    }

    public void Remove(T oldValue) {
        LinkedListElement<T> oldElement = Find(oldValue);
        if(oldElement != null) {
            Remove(oldElement);
        }
    }

    public void RemoveLast() {
        Remove(tail.prev);
    }

    public T Pop() {

        if(size > 0) {

            T r = head.next.value;
            Remove(head.next);

            return r;

        }

        return default(T);

    }

    public LinkedListElement<T> Find(T value) {

        LinkedListElement<T> current = head.next;
        
        while(!current.Equals(tail)) {
            if(current.value.Equals(value))
                return current;
        }

        return null;
    }

}

public class LinkedListElement<T> {

    public LinkedListElement<T> prev, next;
    public T value;

    public LinkedListElement() {
        value = default(T);
        prev = null;
        next = null;
    }

    public LinkedListElement(T value) {
        this.value = value;
        prev = null;
        next = null;
    }

}